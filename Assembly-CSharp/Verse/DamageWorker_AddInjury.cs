using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF0 RID: 3312
	public class DamageWorker_AddInjury : DamageWorker
	{
		// Token: 0x060048FE RID: 18686 RVA: 0x002658EC File Offset: 0x00263CEC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			DamageWorker.DamageResult result;
			if (pawn == null)
			{
				result = base.Apply(dinfo, thing);
			}
			else
			{
				result = this.ApplyToPawn(dinfo, pawn);
			}
			return result;
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x00265924 File Offset: 0x00263D24
		private DamageWorker.DamageResult ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			DamageWorker.DamageResult result;
			if (dinfo.Amount <= 0f)
			{
				result = damageResult;
			}
			else if (!DebugSettings.enablePlayerDamage && pawn.Faction == Faction.OfPlayer)
			{
				result = damageResult;
			}
			else
			{
				Map mapHeld = pawn.MapHeld;
				bool spawnedOrAnyParentSpawned = pawn.SpawnedOrAnyParentSpawned;
				if (dinfo.AllowDamagePropagation && dinfo.Amount >= (float)dinfo.Def.minDamageToFragment)
				{
					int num = Rand.RangeInclusive(2, 4);
					for (int i = 0; i < num; i++)
					{
						DamageInfo dinfo2 = dinfo;
						dinfo2.SetAmount(dinfo.Amount / (float)num);
						this.ApplyDamageToPart(dinfo2, pawn, damageResult);
					}
				}
				else
				{
					this.ApplyDamageToPart(dinfo, pawn, damageResult);
					this.ApplySmallPawnDamagePropagation(dinfo, pawn, damageResult);
				}
				if (damageResult.wounded)
				{
					DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
					pawn.Drawer.Notify_DamageApplied(dinfo);
				}
				if (damageResult.headshot && pawn.Spawned)
				{
					MoteMaker.ThrowText(new Vector3((float)pawn.Position.x + 1f, (float)pawn.Position.y, (float)pawn.Position.z + 1f), pawn.Map, "Headshot".Translate(), Color.white, -1f);
					if (dinfo.Instigator != null)
					{
						Pawn pawn2 = dinfo.Instigator as Pawn;
						if (pawn2 != null)
						{
							pawn2.records.Increment(RecordDefOf.Headshots);
						}
					}
				}
				if (damageResult.deflected)
				{
					EffecterDef effecterDef;
					if (damageResult.deflectedByMetalArmor && dinfo.Def.canUseDeflectMetalEffect)
					{
						if (dinfo.Def == DamageDefOf.Bullet)
						{
							effecterDef = EffecterDefOf.Deflect_Metal_Bullet;
						}
						else
						{
							effecterDef = EffecterDefOf.Deflect_Metal;
						}
					}
					else if (dinfo.Def == DamageDefOf.Bullet)
					{
						effecterDef = EffecterDefOf.Deflect_General_Bullet;
					}
					else
					{
						effecterDef = EffecterDefOf.Deflect_General;
					}
					if (pawn.health.deflectionEffecter == null || pawn.health.deflectionEffecter.def != effecterDef)
					{
						if (pawn.health.deflectionEffecter != null)
						{
							pawn.health.deflectionEffecter.Cleanup();
							pawn.health.deflectionEffecter = null;
						}
						pawn.health.deflectionEffecter = effecterDef.Spawn();
					}
					pawn.health.deflectionEffecter.Trigger(pawn, dinfo.Instigator ?? pawn);
					pawn.Drawer.Notify_DamageDeflected(dinfo);
				}
				else if (spawnedOrAnyParentSpawned)
				{
					ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
				}
				result = damageResult;
			}
			return result;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00265C10 File Offset: 0x00264010
		private void CheckApplySpreadDamage(DamageInfo dinfo, Thing t)
		{
			if (dinfo.Def != DamageDefOf.Flame || t.FlammableNow)
			{
				if (Rand.Chance(0.5f))
				{
					dinfo.SetAmount((float)Mathf.CeilToInt(dinfo.Amount * Rand.Range(0.35f, 0.7f)));
					t.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x00265C7C File Offset: 0x0026407C
		private void ApplySmallPawnDamagePropagation(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			if (dinfo.AllowDamagePropagation)
			{
				if (result.LastHitPart != null && dinfo.Def.harmsHealth && result.LastHitPart != pawn.RaceProps.body.corePart && result.LastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.LastHitPart.parent) > 0f && dinfo.Amount >= 10f && pawn.HealthScale <= 0.5001f)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(result.LastHitPart.parent);
					this.ApplyDamageToPart(dinfo2, pawn, result);
				}
			}
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x00265D48 File Offset: 0x00264148
		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = this.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo != null)
			{
				dinfo.SetHitPart(exactPartFromDamageInfo);
				float num = dinfo.Amount;
				bool flag = !dinfo.InstantPermanentInjury;
				bool deflectedByMetalArmor = false;
				if (flag)
				{
					num = ArmorUtility.GetPostArmorDamage(pawn, num, dinfo.HitPart, dinfo.Def, out deflectedByMetalArmor);
				}
				if (num <= 0f)
				{
					result.AddPart(pawn, dinfo.HitPart);
					result.deflected = true;
					result.deflectedByMetalArmor = deflectedByMetalArmor;
				}
				else
				{
					if (DamageWorker_AddInjury.IsHeadshot(dinfo, pawn))
					{
						result.headshot = true;
					}
					if (dinfo.InstantPermanentInjury)
					{
						HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
						if (hediffDefFromDamage.CompPropsFor(typeof(HediffComp_GetsPermanent)) == null || dinfo.HitPart.def.permanentInjuryBaseChance == 0f || dinfo.HitPart.def.IsSolid(dinfo.HitPart, pawn.health.hediffSet.hediffs) || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart))
						{
							return;
						}
					}
					if (!dinfo.AllowDamagePropagation)
					{
						this.FinalizeAndAddInjury(pawn, num, dinfo, result);
					}
					else
					{
						this.ApplySpecialEffectsToPart(pawn, num, dinfo, result);
					}
				}
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x00265EB1 File Offset: 0x002642B1
		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, result);
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x00265ED8 File Offset: 0x002642D8
		protected float FinalizeAndAddInjury(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			float result2;
			if (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart))
			{
				result2 = 0f;
			}
			else
			{
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
				Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
				hediff_Injury.Part = dinfo.HitPart;
				hediff_Injury.source = dinfo.Weapon;
				hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
				hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
				hediff_Injury.Severity = totalDamage;
				if (dinfo.InstantPermanentInjury)
				{
					HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff_Injury.TryGetComp<HediffComp_GetsPermanent>();
					if (hediffComp_GetsPermanent != null)
					{
						hediffComp_GetsPermanent.IsPermanent = true;
					}
					else
					{
						Log.Error(string.Concat(new object[]
						{
							"Tried to create instant permanent injury on Hediff without a GetsPermanent comp: ",
							hediffDefFromDamage,
							" on ",
							pawn
						}), false);
					}
				}
				result2 = this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
			}
			return result2;
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x00265FD0 File Offset: 0x002643D0
		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			this.CalculatePermanentInjuryDamageThreshold(pawn, injury);
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo), result);
			float num = Mathf.Min(injury.Severity, pawn.health.hediffSet.GetPartHealth(injury.Part));
			result.totalDamageDealt += num;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			result.AddHediff(injury);
			return num;
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x00266054 File Offset: 0x00264454
		private void CalculatePermanentInjuryDamageThreshold(Pawn pawn, Hediff_Injury injury)
		{
			HediffCompProperties_GetsPermanent hediffCompProperties_GetsPermanent = injury.def.CompProps<HediffCompProperties_GetsPermanent>();
			if (hediffCompProperties_GetsPermanent != null)
			{
				if (!injury.Part.def.IsSolid(injury.Part, pawn.health.hediffSet.hediffs) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(injury.Part) && !injury.IsPermanent() && injury.Part.def.permanentInjuryBaseChance >= 1E-05f)
				{
					bool isDelicate = injury.Part.def.IsDelicate;
					if ((Rand.Value <= injury.Part.def.permanentInjuryBaseChance * hediffCompProperties_GetsPermanent.becomePermanentChance && injury.Severity >= injury.Part.def.GetMaxHealth(pawn) * 0.25f && injury.Severity >= 7f) || isDelicate)
					{
						HediffComp_GetsPermanent hediffComp_GetsPermanent = injury.TryGetComp<HediffComp_GetsPermanent>();
						float num = 1f;
						float num2 = injury.Severity / 2f;
						if (num <= num2)
						{
							hediffComp_GetsPermanent.permanentDamageThreshold = Rand.Range(num, num2);
						}
						if (isDelicate)
						{
							hediffComp_GetsPermanent.permanentDamageThreshold = injury.Severity;
							hediffComp_GetsPermanent.IsPermanent = true;
						}
					}
				}
			}
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x002661B0 File Offset: 0x002645B0
		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, float totalDamage, DamageWorker.DamageResult result)
		{
			if (dinfo.AllowDamagePropagation)
			{
				if (dinfo.Def.harmAllLayersUntilOutside && dinfo.HitPart.depth == BodyPartDepth.Inside)
				{
					BodyPartRecord parent = dinfo.HitPart.parent;
					do
					{
						if (pawn.health.hediffSet.GetPartHealth(parent) != 0f)
						{
							HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent);
							Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
							hediff_Injury.Part = parent;
							hediff_Injury.source = dinfo.Weapon;
							hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
							hediff_Injury.Severity = totalDamage;
							if (hediff_Injury.Severity <= 0f)
							{
								hediff_Injury.Severity = 1f;
							}
							this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
						}
						if (parent.depth == BodyPartDepth.Outside)
						{
							break;
						}
						parent = parent.parent;
					}
					while (parent != null);
				}
			}
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x002662B8 File Offset: 0x002646B8
		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantPermanentInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def == DamageDefOf.Bullet;
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x0026630C File Offset: 0x0026470C
		private BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			BodyPartRecord result;
			if (dinfo.HitPart != null)
			{
				result = ((!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Any((BodyPartRecord x) => x == dinfo.HitPart)) ? null : dinfo.HitPart);
			}
			else
			{
				BodyPartRecord bodyPartRecord = this.ChooseHitPart(dinfo, pawn);
				if (bodyPartRecord == null)
				{
					Log.Warning("ChooseHitPart returned null (any part).", false);
				}
				result = bodyPartRecord;
			}
			return result;
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x002663A0 File Offset: 0x002647A0
		protected virtual BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth);
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x002663DC File Offset: 0x002647DC
		private static void PlayWoundedVoiceSound(DamageInfo dinfo, Pawn pawn)
		{
			if (!pawn.Dead)
			{
				if (!dinfo.InstantPermanentInjury)
				{
					if (pawn.SpawnedOrAnyParentSpawned)
					{
						if (dinfo.Def.externalViolence)
						{
							LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge ls) => ls.soundWounded, 1f);
						}
					}
				}
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00266454 File Offset: 0x00264854
		protected float ReduceDamageToPreserveOutsideParts(float postArmorDamage, DamageInfo dinfo, Pawn pawn)
		{
			float result;
			if (!DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(dinfo.HitPart))
			{
				result = postArmorDamage;
			}
			else
			{
				float partHealth = pawn.health.hediffSet.GetPartHealth(dinfo.HitPart);
				if (postArmorDamage < partHealth)
				{
					result = postArmorDamage;
				}
				else
				{
					float maxHealth = dinfo.HitPart.def.GetMaxHealth(pawn);
					float num = postArmorDamage - partHealth;
					float x = num / maxHealth;
					float num2 = DamageWorker_AddInjury.PartBlowOffChancePerOverkillDamagePercent.Evaluate(x);
					if (Rand.Value < num2)
					{
						result = postArmorDamage;
					}
					else
					{
						postArmorDamage = (result = partHealth - 1f);
					}
				}
			}
			return result;
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x002664F0 File Offset: 0x002648F0
		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}

		// Token: 0x0400318A RID: 12682
		private const float SpreadDamageChance = 0.5f;

		// Token: 0x0400318B RID: 12683
		private static readonly SimpleCurve PartBlowOffChancePerOverkillDamagePercent = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0f),
				true
			},
			{
				new CurvePoint(0.7f, 1f),
				true
			}
		};
	}
}

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class DamageWorker_AddInjury : DamageWorker
	{
		[CompilerGenerated]
		private static Func<LifeStageAge, SoundDef> <>f__am$cache0;

		public DamageWorker_AddInjury()
		{
		}

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
				if ((damageResult.deflected || damageResult.diminished) && spawnedOrAnyParentSpawned)
				{
					EffecterDef effecterDef;
					if (damageResult.deflected)
					{
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
					}
					else if (damageResult.diminishedByMetalArmor)
					{
						effecterDef = EffecterDefOf.DamageDiminished_Metal;
					}
					else
					{
						effecterDef = EffecterDefOf.DamageDiminished_General;
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
					if (damageResult.deflected)
					{
						pawn.Drawer.Notify_DamageDeflected(dinfo);
					}
				}
				if (!damageResult.deflected && spawnedOrAnyParentSpawned)
				{
					ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
				}
				result = damageResult;
			}
			return result;
		}

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
					DamageDef def = dinfo.Def;
					bool diminishedByMetalArmor;
					num = ArmorUtility.GetPostArmorDamage(pawn, num, dinfo.ArmorPenetrationInt, dinfo.HitPart, ref def, out deflectedByMetalArmor, out diminishedByMetalArmor);
					dinfo.Def = def;
					if (num < dinfo.Amount)
					{
						result.diminished = true;
						result.diminishedByMetalArmor = diminishedByMetalArmor;
					}
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
						if (hediffDefFromDamage.CompPropsFor(typeof(HediffComp_GetsPermanent)) == null || dinfo.HitPart.def.permanentInjuryChanceFactor == 0f || dinfo.HitPart.def.IsSolid(dinfo.HitPart, pawn.health.hediffSet.hediffs) || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart))
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

		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, result);
		}

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

		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = injury.TryGetComp<HediffComp_GetsPermanent>();
			if (hediffComp_GetsPermanent != null)
			{
				hediffComp_GetsPermanent.PreFinalizeInjury();
			}
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo), result);
			float num = Mathf.Min(injury.Severity, pawn.health.hediffSet.GetPartHealth(injury.Part));
			result.totalDamageDealt += num;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			result.AddHediff(injury);
			return num;
		}

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

		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantPermanentInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def == DamageDefOf.Bullet;
		}

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

		protected virtual BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth);
		}

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
					float f = num / maxHealth;
					float chance = this.def.overkillPctToDestroyPart.InverseLerpThroughRange(f);
					if (Rand.Chance(chance))
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

		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}

		[CompilerGenerated]
		private static SoundDef <PlayWoundedVoiceSound>m__0(LifeStageAge ls)
		{
			return ls.soundWounded;
		}

		[CompilerGenerated]
		private sealed class <GetExactPartFromDamageInfo>c__AnonStorey0
		{
			internal DamageInfo dinfo;

			public <GetExactPartFromDamageInfo>c__AnonStorey0()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x == this.dinfo.HitPart;
			}
		}
	}
}

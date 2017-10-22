using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class DamageWorker_AddInjury : DamageWorker
	{
		private const float SpreadDamageChance = 0.5f;

		public override DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return (pawn != null) ? this.ApplyToPawn(dinfo, pawn) : base.Apply(dinfo, thing);
		}

		private DamageResult ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			DamageResult damageResult = DamageResult.MakeNew();
			DamageResult result;
			if (dinfo.Amount <= 0)
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
				if (dinfo.Def.spreadOut)
				{
					if (pawn.apparel != null)
					{
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int num = wornApparel.Count - 1; num >= 0; num--)
						{
							this.CheckApplySpreadDamage(dinfo, wornApparel[num]);
						}
					}
					if (pawn.equipment != null && pawn.equipment.Primary != null)
					{
						this.CheckApplySpreadDamage(dinfo, pawn.equipment.Primary);
					}
					if (pawn.inventory != null)
					{
						ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
						for (int num2 = innerContainer.Count - 1; num2 >= 0; num2--)
						{
							this.CheckApplySpreadDamage(dinfo, innerContainer[num2]);
						}
					}
				}
				if (this.ShouldFragmentDamageForDamageType(dinfo))
				{
					int num3 = Rand.RangeInclusive(3, 4);
					for (int num4 = 0; num4 < num3; num4++)
					{
						DamageInfo dinfo2 = dinfo;
						dinfo2.SetAmount(dinfo.Amount / num3);
						this.ApplyDamageToPart(dinfo2, pawn, ref damageResult);
					}
				}
				else
				{
					this.ApplyDamageToPart(dinfo, pawn, ref damageResult);
					this.CheckDuplicateSmallPawnDamageToPartParent(dinfo, pawn, ref damageResult);
				}
				if (damageResult.wounded)
				{
					DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
					pawn.Drawer.Notify_DamageApplied(dinfo);
					DamageWorker_AddInjury.InformPsychology(dinfo, pawn);
				}
				if (damageResult.headshot && pawn.Spawned)
				{
					IntVec3 position = pawn.Position;
					double x = (float)position.x + 1.0;
					IntVec3 position2 = pawn.Position;
					float y = (float)position2.y;
					IntVec3 position3 = pawn.Position;
					MoteMaker.ThrowText(new Vector3((float)x, y, (float)((float)position3.z + 1.0)), pawn.Map, "Headshot".Translate(), Color.white, -1f);
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
					if (pawn.health.deflectionEffecter == null)
					{
						pawn.health.deflectionEffecter = EffecterDefOf.ArmorDeflect.Spawn();
					}
					pawn.health.deflectionEffecter.Trigger((Thing)pawn, (Thing)pawn);
				}
				else if (spawnedOrAnyParentSpawned)
				{
					ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
				}
				result = damageResult;
			}
			return result;
		}

		private void CheckApplySpreadDamage(DamageInfo dinfo, Thing t)
		{
			if (dinfo.Def == DamageDefOf.Flame && !t.FlammableNow)
				return;
			if (UnityEngine.Random.value < 0.5)
			{
				dinfo.SetAmount(Mathf.CeilToInt((float)dinfo.Amount * Rand.Range(0.35f, 0.7f)));
				t.TakeDamage(dinfo);
			}
		}

		private bool ShouldFragmentDamageForDamageType(DamageInfo dinfo)
		{
			return (byte)(dinfo.AllowDamagePropagation ? ((dinfo.Amount >= 9) ? (dinfo.Def.spreadOut ? 1 : 0) : 0) : 0) != 0;
		}

		private void CheckDuplicateSmallPawnDamageToPartParent(DamageInfo dinfo, Pawn pawn, ref DamageResult result)
		{
			if (dinfo.AllowDamagePropagation && result.LastHitPart != null && dinfo.Def.harmsHealth && result.LastHitPart != pawn.RaceProps.body.corePart && result.LastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.LastHitPart.parent) > 0.0 && dinfo.Amount >= 10 && pawn.HealthScale <= 0.50010001659393311)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(result.LastHitPart.parent);
				this.ApplyDamageToPart(dinfo2, pawn, ref result);
			}
		}

		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, ref DamageResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = this.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo != null)
			{
				dinfo.SetHitPart(exactPartFromDamageInfo);
				int num = dinfo.Amount;
				if (!dinfo.InstantOldInjury)
				{
					num = ArmorUtility.GetPostArmorDamage(pawn, dinfo.Amount, dinfo.HitPart, dinfo.Def);
				}
				if (num <= 0)
				{
					result.deflected = true;
				}
				else
				{
					if (DamageWorker_AddInjury.IsHeadshot(dinfo, pawn))
					{
						result.headshot = true;
					}
					if (dinfo.InstantOldInjury)
					{
						HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
						if (hediffDefFromDamage.CompPropsFor(typeof(HediffComp_GetsOld)) == null)
							return;
						if (dinfo.HitPart.def.oldInjuryBaseChance == 0.0)
							return;
						if (dinfo.HitPart.def.IsSolid(dinfo.HitPart, pawn.health.hediffSet.hediffs))
							return;
						if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart))
							return;
					}
					if (!dinfo.AllowDamagePropagation)
					{
						this.FinalizeAndAddInjury(pawn, (float)num, dinfo, ref result);
					}
					else
					{
						this.ApplySpecialEffectsToPart(pawn, (float)num, dinfo, ref result);
					}
				}
			}
		}

		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, ref DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, ref result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, ref result);
		}

		protected float FinalizeAndAddInjury(Pawn pawn, float totalDamage, DamageInfo dinfo, ref DamageResult result)
		{
			HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
			Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
			hediff_Injury.Part = dinfo.HitPart;
			hediff_Injury.source = dinfo.Weapon;
			hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
			hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
			hediff_Injury.Severity = totalDamage;
			if (dinfo.InstantOldInjury)
			{
				HediffComp_GetsOld hediffComp_GetsOld = hediff_Injury.TryGetComp<HediffComp_GetsOld>();
				if (hediffComp_GetsOld != null)
				{
					hediffComp_GetsOld.IsOld = true;
				}
				else
				{
					Log.Error("Tried to create instant old injury on Hediff without a GetsOld comp: " + hediffDefFromDamage + " on " + pawn);
				}
			}
			return this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, ref result);
		}

		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, ref DamageResult result)
		{
			this.CalculateOldInjuryDamageThreshold(pawn, injury);
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo));
			float num = Mathf.Min(injury.Severity, pawn.health.hediffSet.GetPartHealth(injury.Part));
			result.totalDamageDealt += num;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			return num;
		}

		private void CalculateOldInjuryDamageThreshold(Pawn pawn, Hediff_Injury injury)
		{
			HediffCompProperties_GetsOld hediffCompProperties_GetsOld = injury.def.CompProps<HediffCompProperties_GetsOld>();
			if (hediffCompProperties_GetsOld != null && !injury.Part.def.IsSolid(injury.Part, pawn.health.hediffSet.hediffs) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(injury.Part) && !injury.IsOld() && !(injury.Part.def.oldInjuryBaseChance < 9.9999997473787516E-06))
			{
				bool isDelicate = injury.Part.def.IsDelicate;
				if ((!(Rand.Value <= injury.Part.def.oldInjuryBaseChance * hediffCompProperties_GetsOld.becomeOldChance) || !(injury.Severity >= injury.Part.def.GetMaxHealth(pawn) * 0.25) || !(injury.Severity >= 7.0)) && !isDelicate)
					return;
				HediffComp_GetsOld hediffComp_GetsOld = injury.TryGetComp<HediffComp_GetsOld>();
				float num = 1f;
				float num2 = (float)(injury.Severity / 2.0);
				if (num <= num2)
				{
					hediffComp_GetsOld.oldDamageThreshold = Rand.Range(num, num2);
				}
				if (isDelicate)
				{
					hediffComp_GetsOld.oldDamageThreshold = injury.Severity;
					hediffComp_GetsOld.IsOld = true;
				}
			}
		}

		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, float totalDamage, ref DamageResult result)
		{
			if (dinfo.AllowDamagePropagation && dinfo.Def.harmAllLayersUntilOutside && dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				BodyPartRecord parent = dinfo.HitPart.parent;
				while (true)
				{
					if (pawn.health.hediffSet.GetPartHealth(parent) != 0.0)
					{
						HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent);
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Part = parent;
						hediff_Injury.source = dinfo.Weapon;
						hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
						hediff_Injury.Severity = totalDamage;
						if (hediff_Injury.Severity <= 0.0)
						{
							hediff_Injury.Severity = 1f;
						}
						this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, ref result);
					}
					if (parent.depth != BodyPartDepth.Outside)
					{
						parent = parent.parent;
						if (parent == null)
							break;
						continue;
					}
					break;
				}
			}
		}

		private static void InformPsychology(DamageInfo dinfo, Pawn pawn)
		{
		}

		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantOldInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def == DamageDefOf.Bullet;
		}

		private BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			BodyPartRecord result;
			if (dinfo.HitPart != null)
			{
				result = ((!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Any((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x == dinfo.HitPart))) ? null : dinfo.HitPart);
			}
			else
			{
				BodyPartRecord bodyPartRecord = this.ChooseHitPart(dinfo, pawn);
				if (bodyPartRecord == null)
				{
					Log.Warning("GetRandomNotMissingPart returned null (any part).");
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
			if (!pawn.Dead && !dinfo.InstantOldInjury && pawn.SpawnedOrAnyParentSpawned && dinfo.Def.externalViolence)
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (Func<LifeStageAge, SoundDef>)((LifeStageAge ls) => ls.soundWounded), 1f);
			}
		}

		protected float ReduceDamageToPreserveOutsideParts(float postArmorDamage, DamageInfo dinfo, Pawn pawn)
		{
			if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(dinfo.HitPart))
			{
				int num = Mathf.FloorToInt(pawn.health.hediffSet.GetPartHealth(dinfo.HitPart));
				if ((float)num >= 6.0 && postArmorDamage >= (float)num && postArmorDamage < (float)num * 1.8999999761581421)
				{
					postArmorDamage = (float)(num - 1);
				}
			}
			return postArmorDamage;
		}

		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}
	}
}

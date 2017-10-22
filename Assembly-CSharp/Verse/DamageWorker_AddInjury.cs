using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class DamageWorker_AddInjury : DamageWorker
	{
		private struct LocalInjuryResult
		{
			public bool wounded;

			public bool headshot;

			public bool deflected;

			public BodyPartRecord lastHitPart;

			public float totalDamageDealt;

			public static LocalInjuryResult MakeNew()
			{
				return new LocalInjuryResult
				{
					wounded = false,
					headshot = false,
					deflected = false,
					lastHitPart = null,
					totalDamageDealt = 0f
				};
			}
		}

		private const float SpreadDamageChance = 0.5f;

		public override float Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return base.Apply(dinfo, thing);
			}
			return this.ApplyToPawn(dinfo, pawn);
		}

		private float ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			if (dinfo.Amount <= 0)
			{
				return 0f;
			}
			if (!DebugSettings.enablePlayerDamage && pawn.Faction == Faction.OfPlayer)
			{
				return 0f;
			}
			LocalInjuryResult localInjuryResult = LocalInjuryResult.MakeNew();
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
					this.ApplyDamageToPart(dinfo2, pawn, ref localInjuryResult);
				}
			}
			else
			{
				this.ApplyDamageToPart(dinfo, pawn, ref localInjuryResult);
				this.CheckDuplicateSmallPawnDamageToPartParent(dinfo, pawn, ref localInjuryResult);
			}
			if (localInjuryResult.wounded)
			{
				DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
				pawn.Drawer.Notify_DamageApplied(dinfo);
				DamageWorker_AddInjury.InformPsychology(dinfo, pawn);
			}
			if (localInjuryResult.headshot && pawn.Spawned)
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
			if (localInjuryResult.deflected)
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
			return localInjuryResult.totalDamageDealt;
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
			if (!dinfo.AllowDamagePropagation)
			{
				return false;
			}
			if (dinfo.Amount < 9)
			{
				return false;
			}
			if (!dinfo.Def.spreadOut)
			{
				return false;
			}
			return true;
		}

		private void CheckDuplicateSmallPawnDamageToPartParent(DamageInfo dinfo, Pawn pawn, ref LocalInjuryResult result)
		{
			if (dinfo.AllowDamagePropagation && result.lastHitPart != null && dinfo.Def.harmsHealth && result.lastHitPart != pawn.RaceProps.body.corePart && result.lastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.lastHitPart.parent) > 0.0 && dinfo.Amount >= 10 && pawn.HealthScale <= 0.50010001659393311)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetForcedHitPart(result.lastHitPart.parent);
				this.ApplyDamageToPart(dinfo2, pawn, ref result);
			}
		}

		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, ref LocalInjuryResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = DamageWorker_AddInjury.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo != null)
			{
				int num = dinfo.Amount;
				bool flag = !dinfo.InstantOldInjury;
				if (flag)
				{
					num = ArmorUtility.GetPostArmorDamage(pawn, dinfo.Amount, exactPartFromDamageInfo, dinfo.Def);
				}
				if (num <= 0)
				{
					result.deflected = true;
				}
				else
				{
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, exactPartFromDamageInfo);
					Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
					hediff_Injury.Part = exactPartFromDamageInfo;
					hediff_Injury.source = dinfo.WeaponGear;
					hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
					hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
					hediff_Injury.Severity = (float)num;
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
					result.wounded = true;
					result.lastHitPart = hediff_Injury.Part;
					if (DamageWorker_AddInjury.IsHeadshot(dinfo, hediff_Injury, pawn))
					{
						result.headshot = true;
					}
					if (dinfo.InstantOldInjury)
					{
						if (hediff_Injury.def.CompPropsFor(typeof(HediffComp_GetsOld)) == null)
							return;
						if (hediff_Injury.Part.def.oldInjuryBaseChance == 0.0)
							return;
						if (hediff_Injury.Part.def.IsSolid(hediff_Injury.Part, pawn.health.hediffSet.hediffs))
							return;
						if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_Injury.Part))
							return;
					}
					this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, ref result);
					this.CheckPropagateDamageToInnerSolidParts(dinfo, pawn, hediff_Injury, flag, ref result);
					this.CheckDuplicateDamageToOuterParts(dinfo, pawn, hediff_Injury, flag, ref result);
				}
			}
		}

		private void FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, ref LocalInjuryResult result)
		{
			this.CalculateOldInjuryDamageThreshold(pawn, injury);
			result.totalDamageDealt += Mathf.Min(injury.Severity, pawn.health.hediffSet.GetPartHealth(injury.Part));
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo));
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

		private void CheckPropagateDamageToInnerSolidParts(DamageInfo dinfo, Pawn pawn, Hediff_Injury injury, bool involveArmor, ref LocalInjuryResult result)
		{
			if (dinfo.AllowDamagePropagation && Rand.Value < HealthTunings.ChanceToAdditionallyDamageInnerSolidPart && dinfo.Def.hasChanceToAdditionallyDamageInnerSolidParts && !injury.Part.def.IsSolid(injury.Part, pawn.health.hediffSet.hediffs) && injury.Part.depth == BodyPartDepth.Outside)
			{
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where x.parent == injury.Part && x.def.IsSolid(x, pawn.health.hediffSet.hediffs) && x.depth == BodyPartDepth.Inside
				select x;
				BodyPartRecord part = default(BodyPartRecord);
				if (source.TryRandomElementByWeight<BodyPartRecord>((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbs), out part))
				{
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, part);
					Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
					hediff_Injury.Part = part;
					hediff_Injury.source = injury.source;
					hediff_Injury.sourceBodyPartGroup = injury.sourceBodyPartGroup;
					hediff_Injury.Severity = (float)(dinfo.Amount / 2);
					if (involveArmor)
					{
						hediff_Injury.Severity = (float)ArmorUtility.GetPostArmorDamage(pawn, dinfo.Amount / 2, part, dinfo.Def);
					}
					if (!(hediff_Injury.Severity <= 0.0))
					{
						result.lastHitPart = hediff_Injury.Part;
						this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, ref result);
					}
				}
			}
		}

		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, Hediff_Injury injury, bool involveArmor, ref LocalInjuryResult result)
		{
			if (dinfo.AllowDamagePropagation && dinfo.Def.harmAllLayersUntilOutside && injury.Part.depth == BodyPartDepth.Inside)
			{
				BodyPartRecord parent = injury.Part.parent;
				while (true)
				{
					if (pawn.health.hediffSet.GetPartHealth(parent) != 0.0)
					{
						HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent);
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Part = parent;
						hediff_Injury.source = injury.source;
						hediff_Injury.sourceBodyPartGroup = injury.sourceBodyPartGroup;
						hediff_Injury.Severity = (float)dinfo.Amount;
						if (involveArmor)
						{
							hediff_Injury.Severity = (float)ArmorUtility.GetPostArmorDamage(pawn, dinfo.Amount, parent, dinfo.Def);
						}
						if (hediff_Injury.Severity <= 0.0)
						{
							hediff_Injury.Severity = 1f;
						}
						result.lastHitPart = hediff_Injury.Part;
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

		private static bool IsHeadshot(DamageInfo dinfo, Hediff_Injury injury, Pawn pawn)
		{
			if (dinfo.InstantOldInjury)
			{
				return false;
			}
			return injury.Part.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def == DamageDefOf.Bullet;
		}

		private static BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			if (dinfo.ForceHitPart != null)
			{
				return (from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where x == dinfo.ForceHitPart
				select x).FirstOrDefault();
			}
			BodyPartRecord randomNotMissingPart = pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth);
			if (randomNotMissingPart == null)
			{
				Log.Warning("GetRandomNotMissingPart returned null (any part).");
			}
			return randomNotMissingPart;
		}

		private static void PlayWoundedVoiceSound(DamageInfo dinfo, Pawn pawn)
		{
			if (!pawn.Dead && !dinfo.InstantOldInjury && pawn.SpawnedOrAnyParentSpawned && dinfo.Def.externalViolence)
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (Func<LifeStageAge, SoundDef>)((LifeStageAge ls) => ls.soundWounded), 1f);
			}
		}
	}
}

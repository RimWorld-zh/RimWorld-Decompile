using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047F RID: 1151
	[HasDebugOutput]
	internal static class AgeInjuryUtility
	{
		// Token: 0x04000C0F RID: 3087
		private const int MaxPermanentInjuryAge = 100;

		// Token: 0x04000C10 RID: 3088
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		// Token: 0x06001433 RID: 5171 RVA: 0x000B0870 File Offset: 0x000AEC70
		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, int age)
		{
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, age);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000B0894 File Offset: 0x000AEC94
		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, int age)
		{
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if (sets == null)
			{
				yield break;
			}
			for (int i = 0; i < sets.Count; i++)
			{
				List<HediffGiver> givers = sets[i].hediffGivers;
				for (int j = 0; j < givers.Count; j++)
				{
					HediffGiver_Birthday agb = givers[j] as HediffGiver_Birthday;
					if (agb != null)
					{
						float ageFractionOfLifeExpectancy = (float)age / raceDef.race.lifeExpectancy;
						if (Rand.Value < agb.ageFractionChanceCurve.Evaluate(ageFractionOfLifeExpectancy))
						{
							yield return agb;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000B08C8 File Offset: 0x000AECC8
		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
		{
			float num = (!pawn.RaceProps.IsMechanoid) ? pawn.RaceProps.lifeExpectancy : 2500f;
			float num2 = num / 8f;
			float b = num * 1.5f;
			float chance = (!pawn.RaceProps.Humanlike) ? 0.03f : 0.15f;
			int num3 = 0;
			for (float num4 = num2; num4 < Mathf.Min((float)pawn.ageTracker.AgeBiologicalYears, b); num4 += num2)
			{
				if (Rand.Chance(chance))
				{
					num3++;
				}
			}
			for (int i = 0; i < num3; i++)
			{
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryBaseChance != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)
				select x;
				if (source.Any<BodyPartRecord>())
				{
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					DamageDef dam = AgeInjuryUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dam, pawn, bodyPartRecord);
					if (bodyPartRecord.def.pawnGeneratorCanAmputate && Rand.Chance(0.3f))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = hediffDefFromDamage;
						hediff_MissingPart.Part = bodyPartRecord;
						hediff_MissingPart.IsFresh = false;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart))
						{
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null, null);
							if (pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f))
							{
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList, null);
							}
						}
					}
					else if (bodyPartRecord.def.permanentInjuryBaseChance > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent)))
					{
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Severity = (float)Rand.RangeInclusive(2, 6);
						hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
						hediff_Injury.Part = bodyPartRecord;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury))
						{
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null, null);
						}
					}
				}
			}
			for (int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++)
			{
				foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, j))
				{
					hediffGiver_Birthday.TryApplyAndSimulateSeverityChange(pawn, (float)j, tryNotToKillPawn);
					if (pawn.Dead)
					{
						break;
					}
				}
				if (pawn.Dead)
				{
					break;
				}
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000B0C90 File Offset: 0x000AF090
		private static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			DamageDef result;
			switch (Rand.RangeInclusive(0, 3 + ((!allowFrostbite) ? 0 : 1)))
			{
			case 0:
				result = DamageDefOf.Bullet;
				break;
			case 1:
				result = DamageDefOf.Scratch;
				break;
			case 2:
				result = DamageDefOf.Bite;
				break;
			case 3:
				result = DamageDefOf.Stab;
				break;
			case 4:
				result = DamageDefOf.Frostbite;
				break;
			default:
				throw new Exception();
			}
			return result;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x000B0D14 File Offset: 0x000AF114
		[DebugOutput]
		public static void PermanentInjuryCalculations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for (int i = 0; i < 10; i++)
			{
				stringBuilder.AppendLine("#" + i + ":");
				List<HediffDef> list = new List<HediffDef>();
				for (int j = 0; j < 100; j++)
				{
					foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, j))
					{
						if (!list.Contains(hediffGiver_Birthday.hediff))
						{
							list.Add(hediffGiver_Birthday.hediff);
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"  age ",
								j,
								" - ",
								hediffGiver_Birthday.hediff
							}));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString(), false);
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for (int k = 0; k < 200; k++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				if (pawn.ageTracker.AgeBiologicalYears >= 40)
				{
					stringBuilder.AppendLine(pawn.Name + " age " + pawn.ageTracker.AgeBiologicalYears);
					foreach (Hediff arg in pawn.health.hediffSet.hediffs)
					{
						stringBuilder.AppendLine(" - " + arg);
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}

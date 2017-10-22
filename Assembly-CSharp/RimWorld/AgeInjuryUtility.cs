using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal static class AgeInjuryUtility
	{
		private const int MaxOldInjuryAge = 100;

		private static List<Thing> emptyIngredientsList = new List<Thing>();

		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, int age)
		{
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, age);
		}

		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, int age)
		{
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if (sets != null)
			{
				for (int j = 0; j < sets.Count; j++)
				{
					List<HediffGiver> givers = sets[j].hediffGivers;
					for (int i = 0; i < givers.Count; i++)
					{
						HediffGiver_Birthday agb = givers[i] as HediffGiver_Birthday;
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
			}
		}

		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
		{
			int num = 0;
			for (int num2 = 10; num2 < Mathf.Min(pawn.ageTracker.AgeBiologicalYears, 120); num2 += 10)
			{
				if (Rand.Value < 0.15000000596046448)
				{
					num++;
				}
			}
			for (int num3 = 0; num3 < num; num3++)
			{
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where x.depth == BodyPartDepth.Outside && !Mathf.Approximately(x.def.oldInjuryBaseChance, 0f) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)
				select x;
				if (source.Any())
				{
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.coverageAbs));
					DamageDef dam = AgeInjuryUtility.RandomOldInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0.0 && pawn.RaceProps.ToolUser);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dam, pawn, bodyPartRecord);
					if (bodyPartRecord.def.oldInjuryBaseChance > 0.0 && hediffDefFromDamage.CompPropsFor(typeof(HediffComp_GetsOld)) != null)
					{
						if (Rand.Chance(bodyPartRecord.def.amputateIfGeneratedInjuredChance))
						{
							Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
							hediff_MissingPart.lastInjury = hediffDefFromDamage;
							hediff_MissingPart.TryGetComp<HediffComp_GetsOld>().IsOld = true;
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, default(DamageInfo?));
							if (pawn.RaceProps.Humanlike && (bodyPartRecord.def == BodyPartDefOf.LeftLeg || bodyPartRecord.def == BodyPartDefOf.RightLeg) && Rand.Chance(0.5f))
							{
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList);
							}
						}
						else
						{
							Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
							hediff_Injury.Severity = (float)Rand.RangeInclusive(2, 6);
							hediff_Injury.TryGetComp<HediffComp_GetsOld>().IsOld = true;
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, default(DamageInfo?));
						}
					}
				}
			}
			int num4 = 1;
			while (num4 < pawn.ageTracker.AgeBiologicalYears)
			{
				foreach (HediffGiver_Birthday item in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, num4))
				{
					item.TryApplyAndSimulateSeverityChange(pawn, (float)num4, tryNotToKillPawn);
					if (pawn.Dead)
						break;
				}
				if (!pawn.Dead)
				{
					num4++;
					continue;
				}
				break;
			}
		}

		private static DamageDef RandomOldInjuryDamageType(bool allowFrostbite)
		{
			switch (Rand.RangeInclusive(0, 3 + (allowFrostbite ? 1 : 0)))
			{
			case 0:
			{
				return DamageDefOf.Bullet;
			}
			case 1:
			{
				return DamageDefOf.Scratch;
			}
			case 2:
			{
				return DamageDefOf.Bite;
			}
			case 3:
			{
				return DamageDefOf.Stab;
			}
			case 4:
			{
				return DamageDefOf.Frostbite;
			}
			default:
			{
				throw new Exception();
			}
			}
		}

		public static void LogOldInjuryCalculations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for (int i = 0; i < 10; i++)
			{
				stringBuilder.AppendLine("#" + i + ":");
				List<HediffDef> list = new List<HediffDef>();
				for (int j = 0; j < 100; j++)
				{
					foreach (HediffGiver_Birthday item in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, j))
					{
						if (!list.Contains(item.hediff))
						{
							list.Add(item.hediff);
							stringBuilder.AppendLine("  age " + j + " - " + item.hediff);
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString());
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for (int k = 0; k < 200; k++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				if (pawn.ageTracker.AgeBiologicalYears >= 40)
				{
					stringBuilder.AppendLine(pawn.Name + " age " + pawn.ageTracker.AgeBiologicalYears);
					List<Hediff>.Enumerator enumerator2 = pawn.health.hediffSet.hediffs.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Hediff current2 = enumerator2.Current;
							stringBuilder.AppendLine(" - " + current2);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}

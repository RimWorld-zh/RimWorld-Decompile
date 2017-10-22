using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnAddictionHediffsGenerator
	{
		private const int MaxAddictions = 3;

		private static List<ThingDef> allDrugs = new List<ThingDef>();

		private static readonly FloatRange GeneratedAddictionSeverityRange = new FloatRange(0.6f, 1f);

		private static readonly FloatRange GeneratedToleranceSeverityRange = new FloatRange(0.1f, 0.9f);

		public static void GenerateAddictionsAndTolerancesFor(Pawn pawn)
		{
			if (pawn.RaceProps.IsFlesh && pawn.RaceProps.Humanlike && !pawn.IsTeetotaler())
			{
				PawnAddictionHediffsGenerator.allDrugs.Clear();
				int num = 0;
				while (num < 3 && !(Rand.Value >= pawn.kindDef.chemicalAddictionChance))
				{
					if (!PawnAddictionHediffsGenerator.allDrugs.Any())
					{
						PawnAddictionHediffsGenerator.allDrugs.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
						where x.category == ThingCategory.Item && x.GetCompProperties<CompProperties_Drug>() != null
						select x);
					}
					IEnumerable<ChemicalDef> source = from x in DefDatabase<ChemicalDef>.AllDefsListForReading
					where PawnAddictionHediffsGenerator.PossibleWithTechLevel(x, pawn.Faction) && !AddictionUtility.IsAddicted(pawn, x)
					select x;
					ChemicalDef chemicalDef = default(ChemicalDef);
					if (source.TryRandomElement<ChemicalDef>(out chemicalDef))
					{
						Hediff hediff = HediffMaker.MakeHediff(chemicalDef.addictionHediff, pawn, null);
						hediff.Severity = PawnAddictionHediffsGenerator.GeneratedAddictionSeverityRange.RandomInRange;
						pawn.health.AddHediff(hediff, null, default(DamageInfo?));
						if (chemicalDef.toleranceHediff != null && Rand.Value < chemicalDef.onGeneratedAddictedToleranceChance)
						{
							Hediff hediff2 = HediffMaker.MakeHediff(chemicalDef.toleranceHediff, pawn, null);
							hediff2.Severity = PawnAddictionHediffsGenerator.GeneratedToleranceSeverityRange.RandomInRange;
							pawn.health.AddHediff(hediff2, null, default(DamageInfo?));
						}
						if (chemicalDef.onGeneratedAddictedEvents != null)
						{
							List<HediffGiver_Event>.Enumerator enumerator = chemicalDef.onGeneratedAddictedEvents.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									HediffGiver_Event current = enumerator.Current;
									current.EventOccurred(pawn);
								}
							}
							finally
							{
								((IDisposable)(object)enumerator).Dispose();
							}
						}
						PawnAddictionHediffsGenerator.DoIngestionOutcomeDoers(pawn, chemicalDef);
						num++;
						continue;
					}
					break;
				}
			}
		}

		private static bool PossibleWithTechLevel(ChemicalDef chemical, Faction faction)
		{
			if (faction == null)
			{
				return true;
			}
			return PawnAddictionHediffsGenerator.allDrugs.Any((Predicate<ThingDef>)((ThingDef x) => x.GetCompProperties<CompProperties_Drug>().chemical == chemical && (int)x.techLevel <= (int)faction.def.techLevel));
		}

		private static void DoIngestionOutcomeDoers(Pawn pawn, ChemicalDef chemical)
		{
			for (int i = 0; i < PawnAddictionHediffsGenerator.allDrugs.Count; i++)
			{
				CompProperties_Drug compProperties = PawnAddictionHediffsGenerator.allDrugs[i].GetCompProperties<CompProperties_Drug>();
				if (compProperties.chemical == chemical)
				{
					List<IngestionOutcomeDoer> outcomeDoers = PawnAddictionHediffsGenerator.allDrugs[i].ingestible.outcomeDoers;
					for (int j = 0; j < outcomeDoers.Count; j++)
					{
						if (outcomeDoers[j].doToGeneratedPawnIfAddicted)
						{
							outcomeDoers[j].DoIngestionOutcome(pawn, null);
						}
					}
				}
			}
		}
	}
}

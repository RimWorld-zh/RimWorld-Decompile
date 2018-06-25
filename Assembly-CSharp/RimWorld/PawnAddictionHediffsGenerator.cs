using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PawnAddictionHediffsGenerator
	{
		private static List<ThingDef> allDrugs = new List<ThingDef>();

		private const int MaxAddictions = 3;

		private static readonly FloatRange GeneratedAddictionSeverityRange = new FloatRange(0.6f, 1f);

		private static readonly FloatRange GeneratedToleranceSeverityRange = new FloatRange(0.1f, 0.9f);

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		public static void GenerateAddictionsAndTolerancesFor(Pawn pawn)
		{
			if (pawn.RaceProps.IsFlesh && pawn.RaceProps.Humanlike)
			{
				if (!pawn.IsTeetotaler())
				{
					PawnAddictionHediffsGenerator.allDrugs.Clear();
					for (int i = 0; i < 3; i++)
					{
						if (Rand.Value >= pawn.kindDef.chemicalAddictionChance)
						{
							break;
						}
						if (!PawnAddictionHediffsGenerator.allDrugs.Any<ThingDef>())
						{
							PawnAddictionHediffsGenerator.allDrugs.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
							where x.category == ThingCategory.Item && x.GetCompProperties<CompProperties_Drug>() != null
							select x);
						}
						IEnumerable<ChemicalDef> source = from x in DefDatabase<ChemicalDef>.AllDefsListForReading
						where PawnAddictionHediffsGenerator.PossibleWithTechLevel(x, pawn.Faction) && !AddictionUtility.IsAddicted(pawn, x)
						select x;
						ChemicalDef chemicalDef;
						if (!source.TryRandomElement(out chemicalDef))
						{
							break;
						}
						Hediff hediff = HediffMaker.MakeHediff(chemicalDef.addictionHediff, pawn, null);
						hediff.Severity = PawnAddictionHediffsGenerator.GeneratedAddictionSeverityRange.RandomInRange;
						pawn.health.AddHediff(hediff, null, null, null);
						if (chemicalDef.toleranceHediff != null && Rand.Value < chemicalDef.onGeneratedAddictedToleranceChance)
						{
							Hediff hediff2 = HediffMaker.MakeHediff(chemicalDef.toleranceHediff, pawn, null);
							hediff2.Severity = PawnAddictionHediffsGenerator.GeneratedToleranceSeverityRange.RandomInRange;
							pawn.health.AddHediff(hediff2, null, null, null);
						}
						if (chemicalDef.onGeneratedAddictedEvents != null)
						{
							foreach (HediffGiver_Event hediffGiver_Event in chemicalDef.onGeneratedAddictedEvents)
							{
								hediffGiver_Event.EventOccurred(pawn);
							}
						}
						PawnAddictionHediffsGenerator.DoIngestionOutcomeDoers(pawn, chemicalDef);
					}
				}
			}
		}

		private static bool PossibleWithTechLevel(ChemicalDef chemical, Faction faction)
		{
			return faction == null || PawnAddictionHediffsGenerator.allDrugs.Any((ThingDef x) => x.GetCompProperties<CompProperties_Drug>().chemical == chemical && x.techLevel <= faction.def.techLevel);
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

		// Note: this type is marked as 'beforefieldinit'.
		static PawnAddictionHediffsGenerator()
		{
		}

		[CompilerGenerated]
		private static bool <GenerateAddictionsAndTolerancesFor>m__0(ThingDef x)
		{
			return x.category == ThingCategory.Item && x.GetCompProperties<CompProperties_Drug>() != null;
		}

		[CompilerGenerated]
		private sealed class <GenerateAddictionsAndTolerancesFor>c__AnonStorey0
		{
			internal Pawn pawn;

			public <GenerateAddictionsAndTolerancesFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ChemicalDef x)
			{
				return PawnAddictionHediffsGenerator.PossibleWithTechLevel(x, this.pawn.Faction) && !AddictionUtility.IsAddicted(this.pawn, x);
			}
		}

		[CompilerGenerated]
		private sealed class <PossibleWithTechLevel>c__AnonStorey1
		{
			internal ChemicalDef chemical;

			internal Faction faction;

			public <PossibleWithTechLevel>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.GetCompProperties<CompProperties_Drug>().chemical == this.chemical && x.techLevel <= this.faction.def.techLevel;
			}
		}
	}
}

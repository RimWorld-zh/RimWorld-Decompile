using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000487 RID: 1159
	public static class PawnAddictionHediffsGenerator
	{
		// Token: 0x04000C42 RID: 3138
		private static List<ThingDef> allDrugs = new List<ThingDef>();

		// Token: 0x04000C43 RID: 3139
		private const int MaxAddictions = 3;

		// Token: 0x04000C44 RID: 3140
		private static readonly FloatRange GeneratedAddictionSeverityRange = new FloatRange(0.6f, 1f);

		// Token: 0x04000C45 RID: 3141
		private static readonly FloatRange GeneratedToleranceSeverityRange = new FloatRange(0.1f, 0.9f);

		// Token: 0x06001473 RID: 5235 RVA: 0x000B32D8 File Offset: 0x000B16D8
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

		// Token: 0x06001474 RID: 5236 RVA: 0x000B3504 File Offset: 0x000B1904
		private static bool PossibleWithTechLevel(ChemicalDef chemical, Faction faction)
		{
			return faction == null || PawnAddictionHediffsGenerator.allDrugs.Any((ThingDef x) => x.GetCompProperties<CompProperties_Drug>().chemical == chemical && x.techLevel <= faction.def.techLevel);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000B3558 File Offset: 0x000B1958
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

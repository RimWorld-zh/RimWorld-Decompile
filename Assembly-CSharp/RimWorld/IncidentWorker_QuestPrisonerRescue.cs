using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000355 RID: 853
	public class IncidentWorker_QuestPrisonerRescue : IncidentWorker
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x0007C950 File Offset: 0x0007AD50
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			SitePartDef sitePartDef;
			Faction faction;
			return base.CanFireNowSub(parms) && Find.AnyPlayerHomeMap != null && this.TryFindTile(out num) && SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, out sitePartDef, out faction, null, true, null);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0007C9C4 File Offset: 0x0007ADC4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int tile;
			bool result;
			if (!this.TryFindTile(out tile))
			{
				result = false;
			}
			else
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, null, true, null, true);
				if (site == null)
				{
					result = false;
				}
				else
				{
					site.Tile = tile;
					Pawn pawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(tile, site.Faction);
					site.GetComponent<PrisonerWillingToJoinComp>().pawn.TryAdd(pawn, true);
					int randomInRange = IncidentWorker_QuestPrisonerRescue.TimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					string text;
					string label;
					this.GetLetterText(pawn, site.Faction, randomInRange, out text, out label);
					Find.LetterStack.ReceiveLetter(label, text, this.def.letterDef, site, site.Faction, null);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0007CAA0 File Offset: 0x0007AEA0
		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 18, false, false, -1);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0007CAC4 File Offset: 0x0007AEC4
		private void GetLetterText(Pawn prisoner, Faction siteFaction, int days, out string letter, out string label)
		{
			letter = string.Format(this.def.letterText.AdjustedFor(prisoner), siteFaction.Name, prisoner.ageTracker.AgeBiologicalYears, prisoner.story.Title).CapitalizeFirst();
			if (PawnUtility.EverBeenColonistOrTameAnimal(prisoner))
			{
				letter = letter + "\n\n" + "PawnWasFormerlyColonist".Translate(new object[]
				{
					prisoner.LabelShort
				});
			}
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(Gen.YieldSingle<Pawn>(prisoner), out text, true, false);
			label = this.def.letterLabel;
			if (!text.NullOrEmpty())
			{
				string text2 = letter;
				letter = string.Concat(new string[]
				{
					text2,
					"\n\n",
					"PawnHasTheseRelationshipsWithColonists".Translate(new object[]
					{
						prisoner.LabelShort
					}),
					"\n\n",
					text
				});
				label = label + " " + "RelationshipAppendedLetterSuffix".Translate();
			}
			letter = letter + "\n\n" + "PrisonerRescueTimeout".Translate(new object[]
			{
				days,
				prisoner.LabelShort
			});
		}

		// Token: 0x0400090C RID: 2316
		private const int MinDistance = 2;

		// Token: 0x0400090D RID: 2317
		private const int MaxDistance = 18;

		// Token: 0x0400090E RID: 2318
		private static readonly string PrisonerRescueQuestThreatTag = "PrisonerRescueQuestThreat";

		// Token: 0x0400090F RID: 2319
		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);
	}
}

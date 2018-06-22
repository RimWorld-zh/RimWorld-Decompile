using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000355 RID: 853
	public class IncidentWorker_QuestPrisonerRescue : IncidentWorker
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x0007CB38 File Offset: 0x0007AF38
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			SitePartDef sitePartDef;
			Faction faction;
			return base.CanFireNowSub(parms) && Find.AnyPlayerHomeMap != null && this.TryFindTile(out num) && SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, out sitePartDef, out faction, null, true, null);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0007CBAC File Offset: 0x0007AFAC
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

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0007CC88 File Offset: 0x0007B088
		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 18, false, false, -1);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0007CCAC File Offset: 0x0007B0AC
		private void GetLetterText(Pawn prisoner, Faction siteFaction, int days, out string letter, out string label)
		{
			letter = string.Format(this.def.letterText.AdjustedFor(prisoner, "PAWN"), siteFaction.Name, prisoner.ageTracker.AgeBiologicalYears, prisoner.story.Title).CapitalizeFirst();
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

		// Token: 0x0400090E RID: 2318
		private const int MinDistance = 2;

		// Token: 0x0400090F RID: 2319
		private const int MaxDistance = 18;

		// Token: 0x04000910 RID: 2320
		private static readonly string PrisonerRescueQuestThreatTag = "PrisonerRescueQuestThreat";

		// Token: 0x04000911 RID: 2321
		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);
	}
}

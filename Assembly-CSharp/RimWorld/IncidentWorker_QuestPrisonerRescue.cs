using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000357 RID: 855
	public class IncidentWorker_QuestPrisonerRescue : IncidentWorker
	{
		// Token: 0x04000911 RID: 2321
		private const int MinDistance = 2;

		// Token: 0x04000912 RID: 2322
		private const int MaxDistance = 18;

		// Token: 0x04000913 RID: 2323
		private static readonly string PrisonerRescueQuestThreatTag = "PrisonerRescueQuestThreat";

		// Token: 0x04000914 RID: 2324
		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0007CC90 File Offset: 0x0007B090
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			SitePartDef sitePartDef;
			Faction faction;
			return base.CanFireNowSub(parms) && Find.AnyPlayerHomeMap != null && this.TryFindTile(out num) && SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, out sitePartDef, out faction, null, true, null);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0007CD04 File Offset: 0x0007B104
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

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0007CDE0 File Offset: 0x0007B1E0
		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 18, false, false, -1);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0007CE04 File Offset: 0x0007B204
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
	}
}

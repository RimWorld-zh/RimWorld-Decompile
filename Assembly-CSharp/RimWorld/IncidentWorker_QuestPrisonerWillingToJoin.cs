using RimWorld.Planet;
using System;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestPrisonerWillingToJoin : IncidentWorker
	{
		private const int MinDistance = 2;

		private const int MaxDistance = 20;

		private static readonly string PrisonerWillingToJoinQuestThreatTag = "PrisonerWillingToJoinQuestThreat";

		private static readonly IntRange TimeoutDaysRange = new IntRange(10, 30);

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num = default(int);
			SitePartDef sitePartDef = default(SitePartDef);
			Faction faction = default(Faction);
			return (byte)(base.CanFireNowSub(target) ? ((Find.AnyPlayerHomeMap != null) ? (CommsConsoleUtility.PlayerHasPoweredCommsConsole() ? (this.TryFindTile(out num) ? (SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerWillingToJoin.PrisonerWillingToJoinQuestThreatTag, out sitePartDef, out faction, (Faction)null, true, (Predicate<Faction>)null) ? 1 : 0) : 0) : 0) : 0) : 0) != 0;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int tile = default(int);
			bool result;
			if (!this.TryFindTile(out tile))
			{
				result = false;
			}
			else
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerWillingToJoin.PrisonerWillingToJoinQuestThreatTag, null, true, null);
				if (site == null)
				{
					result = false;
				}
				else
				{
					site.Tile = tile;
					Pawn pawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(tile, site.Faction);
					((WorldObject)site).GetComponent<PrisonerWillingToJoinComp>().pawn.TryAdd(pawn, true);
					int randomInRange = IncidentWorker_QuestPrisonerWillingToJoin.TimeoutDaysRange.RandomInRange;
					((WorldObject)site).GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					Find.LetterStack.ReceiveLetter(base.def.letterLabel, this.GetLetterText(pawn, site.Faction, randomInRange), base.def.letterDef, (WorldObject)site, (string)null);
					result = true;
				}
			}
			return result;
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 20, false, false, -1);
		}

		private string GetLetterText(Pawn prisoner, Faction siteFaction, int days)
		{
			string text = string.Format(base.def.letterText.AdjustedFor(prisoner), siteFaction.Name, prisoner.ageTracker.AgeBiologicalYears, prisoner.story.Title).CapitalizeFirst();
			if (PawnUtility.EverBeenColonistOrTameAnimal(prisoner))
			{
				text = text + "\n\n" + "PawnWasFormerlyColonist".Translate(prisoner.LabelShort);
			}
			string text2 = default(string);
			PawnRelationUtility.Notify_PawnsSeenByPlayer(Gen.YieldSingle(prisoner), out text2, true, false);
			if (!text2.NullOrEmpty())
			{
				string text3 = text;
				text = text3 + "\n\n" + "PawnHasTheseRelationshipsWithColonists".Translate(prisoner.LabelShort) + "\n\n" + text2;
			}
			return text + "\n\n" + "PrisonerRescueTimeout".Translate(days, prisoner.LabelShort);
		}
	}
}

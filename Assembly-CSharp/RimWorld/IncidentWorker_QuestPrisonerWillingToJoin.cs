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
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (!CommsConsoleUtility.PlayerHasPoweredCommsConsole())
			{
				return false;
			}
			int num = default(int);
			if (!this.TryFindTile(out num))
			{
				return false;
			}
			SitePartDef sitePartDef = default(SitePartDef);
			Faction faction = default(Faction);
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerWillingToJoin.PrisonerWillingToJoinQuestThreatTag, out sitePartDef, out faction, (Faction)null, true, (Predicate<Faction>)null))
			{
				return false;
			}
			return true;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int tile = default(int);
			if (!this.TryFindTile(out tile))
			{
				return false;
			}
			Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerWillingToJoin.PrisonerWillingToJoinQuestThreatTag, null, true, null);
			if (site == null)
			{
				return false;
			}
			site.Tile = tile;
			Pawn pawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(tile, site.Faction);
			((WorldObject)site).GetComponent<PrisonerWillingToJoinComp>().pawn.TryAdd(pawn, true);
			int randomInRange = IncidentWorker_QuestPrisonerWillingToJoin.TimeoutDaysRange.RandomInRange;
			((WorldObject)site).GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
			Find.WorldObjects.Add(site);
			Find.LetterStack.ReceiveLetter(base.def.letterLabel, this.GetLetterText(pawn, site.Faction, randomInRange), base.def.letterDef, site, null);
			return true;
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

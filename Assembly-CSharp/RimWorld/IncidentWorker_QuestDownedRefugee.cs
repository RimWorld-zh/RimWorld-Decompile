using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestDownedRefugee : IncidentWorker
	{
		private const float NoSitePartChance = 0.3f;

		private const int MinDistance = 2;

		private const int MaxDistance = 15;

		private static readonly string DownedRefugeeQuestThreatTag = "DownedRefugeeQuestThreat";

		private static readonly IntRange TimeoutDaysRange = new IntRange(5, 10);

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			int num = default(int);
			Faction faction = default(Faction);
			return this.TryFindTile(out num) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.DownedRefugee, (IEnumerable<SitePartDef>)null, out faction, true, (Predicate<Faction>)null);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int tile = default(int);
			if (!this.TryFindTile(out tile))
			{
				return false;
			}
			Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.DownedRefugee, (!Rand.Chance(0.3f)) ? IncidentWorker_QuestDownedRefugee.DownedRefugeeQuestThreatTag : null, null, true, null);
			if (site == null)
			{
				return false;
			}
			site.Tile = tile;
			Pawn pawn = DownedRefugeeQuestUtility.GenerateRefugee(tile);
			((WorldObject)site).GetComponent<DownedRefugeeComp>().pawn.TryAdd(pawn, true);
			int randomInRange = IncidentWorker_QuestDownedRefugee.TimeoutDaysRange.RandomInRange;
			((WorldObject)site).GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
			Find.WorldObjects.Add(site);
			string text = string.Format(base.def.letterText.AdjustedFor(pawn), pawn.Label, randomInRange).CapitalizeFirst();
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative != null)
			{
				PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
				if (mostImportantRelation != null && mostImportantRelation.opinionOffset > 0)
				{
					pawn.relations.relativeInvolvedInRescueQuest = mostImportantColonyRelative;
					text = text + "\n\n" + "RelatedPawnInvolvedInQuest".Translate(mostImportantColonyRelative.LabelShort, mostImportantRelation.GetGenderSpecificLabel(pawn)).AdjustedFor(pawn);
				}
				else
				{
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, pawn);
				}
			}
			if (pawn.relations != null)
			{
				pawn.relations.everSeenByPlayer = true;
			}
			Find.LetterStack.ReceiveLetter(base.def.letterLabel, text, base.def.letterDef, site, null);
			return true;
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 15, true, false, -1);
		}
	}
}

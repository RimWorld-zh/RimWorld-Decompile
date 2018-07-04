using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestDownedRefugee : IncidentWorker
	{
		private const float NoSitePartChance = 0.3f;

		private const int MinDistance = 2;

		private const int MaxDistance = 13;

		private static readonly string DownedRefugeeQuestThreatTag = "DownedRefugeeQuestThreat";

		private static readonly IntRange TimeoutDaysRange = new IntRange(7, 15);

		public IncidentWorker_QuestDownedRefugee()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && this.TryFindTile(out num) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.DownedRefugee, null, out faction, true, null);
		}

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
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.DownedRefugee, (!Rand.Chance(0.3f)) ? IncidentWorker_QuestDownedRefugee.DownedRefugeeQuestThreatTag : null, tile, null, true, null, true);
				if (site == null)
				{
					result = false;
				}
				else
				{
					site.sitePartsKnown = true;
					Pawn pawn = DownedRefugeeQuestUtility.GenerateRefugee(tile);
					site.GetComponent<DownedRefugeeComp>().pawn.TryAdd(pawn, true);
					int randomInRange = IncidentWorker_QuestDownedRefugee.TimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					string text = this.def.letterLabel;
					string text2 = string.Format(this.def.letterText.AdjustedFor(pawn, "PAWN"), new object[]
					{
						randomInRange,
						pawn.ageTracker.AgeBiologicalYears,
						pawn.story.Title,
						SitePartUtility.GetDescriptionDialogue(site, site.parts.FirstOrDefault<SitePart>())
					}).CapitalizeFirst();
					Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
					if (mostImportantColonyRelative != null)
					{
						PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
						if (mostImportantRelation != null && mostImportantRelation.opinionOffset > 0)
						{
							pawn.relations.relativeInvolvedInRescueQuest = mostImportantColonyRelative;
							text2 = text2 + "\n\n" + "RelatedPawnInvolvedInQuest".Translate(new object[]
							{
								mostImportantColonyRelative.LabelShort,
								mostImportantRelation.GetGenderSpecificLabel(pawn)
							}).AdjustedFor(pawn, "PAWN");
						}
						else
						{
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text2, pawn);
						}
						text = text + " " + "RelationshipAppendedLetterSuffix".Translate();
					}
					if (pawn.relations != null)
					{
						pawn.relations.everSeenByPlayer = true;
					}
					Find.LetterStack.ReceiveLetter(text, text2, this.def.letterDef, site, null, null);
					result = true;
				}
			}
			return result;
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 13, true, false, -1);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_QuestDownedRefugee()
		{
		}
	}
}

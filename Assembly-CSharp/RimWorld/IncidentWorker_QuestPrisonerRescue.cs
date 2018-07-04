using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestPrisonerRescue : IncidentWorker
	{
		private const int MinDistance = 2;

		private const int MaxDistance = 18;

		private static readonly string PrisonerRescueQuestThreatTag = "PrisonerRescueQuestThreat";

		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);

		public IncidentWorker_QuestPrisonerRescue()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			SitePartDef sitePartDef;
			Faction faction;
			return base.CanFireNowSub(parms) && Find.AnyPlayerHomeMap != null && this.TryFindTile(out num) && SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, out sitePartDef, out faction, null, true, null);
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
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PrisonerWillingToJoin, IncidentWorker_QuestPrisonerRescue.PrisonerRescueQuestThreatTag, tile, null, true, null, true);
				if (site == null)
				{
					result = false;
				}
				else
				{
					site.sitePartsKnown = true;
					Pawn pawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(tile, site.Faction);
					site.GetComponent<PrisonerWillingToJoinComp>().pawn.TryAdd(pawn, true);
					int randomInRange = IncidentWorker_QuestPrisonerRescue.TimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					string text;
					string label;
					this.GetLetterText(pawn, site, site.parts.FirstOrDefault<SitePart>(), randomInRange, out text, out label);
					Find.LetterStack.ReceiveLetter(label, text, this.def.letterDef, site, site.Faction, null);
					result = true;
				}
			}
			return result;
		}

		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 18, false, false, -1);
		}

		private void GetLetterText(Pawn prisoner, Site site, SitePart sitePart, int days, out string letter, out string label)
		{
			letter = string.Format(this.def.letterText.AdjustedFor(prisoner, "PAWN"), new object[]
			{
				site.Faction.Name,
				prisoner.ageTracker.AgeBiologicalYears,
				prisoner.story.Title,
				SitePartUtility.GetDescriptionDialogue(site, sitePart)
			}).CapitalizeFirst();
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

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_QuestPrisonerRescue()
		{
		}
	}
}

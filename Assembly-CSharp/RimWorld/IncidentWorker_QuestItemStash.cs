using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestItemStash : IncidentWorker
	{
		public IncidentWorker_QuestItemStash()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && (Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined) != null && this.TryFindTile(out num)) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, null, out faction, true, null);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction = parms.faction;
			if (faction == null)
			{
				faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
			}
			bool result;
			int tile;
			SitePartDef sitePart;
			Faction siteFaction;
			if (faction == null)
			{
				result = false;
			}
			else if (!this.TryFindTile(out tile))
			{
				result = false;
			}
			else if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.ItemStash, (!Rand.Chance(0.15f)) ? "ItemStashQuestThreat" : null, out sitePart, out siteFaction, null, true, null))
			{
				result = false;
			}
			else
			{
				int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
				Site site = IncidentWorker_QuestItemStash.CreateSite(tile, sitePart, randomInRange, siteFaction);
				List<Thing> list = this.GenerateItems(siteFaction, site.desiredThreatPoints);
				site.GetComponent<ItemStashContentsComp>().contents.TryAddRangeOrTransfer(list, false, false);
				string letterText = this.GetLetterText(faction, list, randomInRange, site, site.parts.FirstOrDefault<SitePart>());
				Find.LetterStack.ReceiveLetter(this.def.letterLabel, letterText, this.def.letterDef, site, faction, null);
				result = true;
			}
			return result;
		}

		private bool TryFindTile(out int tile)
		{
			IntRange itemStashQuestSiteDistanceRange = SiteTuning.ItemStashQuestSiteDistanceRange;
			return TileFinder.TryFindNewSiteTile(out tile, itemStashQuestSiteDistanceRange.min, itemStashQuestSiteDistanceRange.max, false, true, -1);
		}

		protected virtual List<Thing> GenerateItems(Faction siteFaction, float siteThreatPoints)
		{
			float val = SiteTuning.QuestRewardMarketValueThreatPointsFactor.Evaluate(siteThreatPoints);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(SiteTuning.ItemStashQuestMarketValueRange * val);
			return ThingSetMakerDefOf.Reward_ItemStashQuestContents.root.Generate(parms);
		}

		public static Site CreateSite(int tile, SitePartDef sitePart, int days, Faction siteFaction)
		{
			Site site = SiteMaker.MakeSite(SiteCoreDefOf.ItemStash, sitePart, tile, siteFaction, true, null);
			site.sitePartsKnown = true;
			site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
			Find.WorldObjects.Add(site);
			return site;
		}

		private string GetLetterText(Faction alliedFaction, List<Thing> items, int days, Site site, SitePart sitePart)
		{
			string text = string.Format(this.def.letterText, new object[]
			{
				alliedFaction.leader.LabelShort,
				alliedFaction.def.leaderTitle,
				alliedFaction.Name,
				GenLabel.ThingsLabel(items),
				days.ToString(),
				SitePartUtility.GetDescriptionDialogue(site, sitePart)
			}).CapitalizeFirst();
			if (items.Count == 1 || (items.Count >= 2 && items.All((Thing x) => x.def == items[0].def)))
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n\n---\n\n",
					items[0].LabelCapNoCount,
					": ",
					items[0].DescriptionFlavor
				});
			}
			return text;
		}

		[CompilerGenerated]
		private sealed class <GetLetterText>c__AnonStorey0
		{
			internal List<Thing> items;

			public <GetLetterText>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.def == this.items[0].def;
			}
		}
	}
}

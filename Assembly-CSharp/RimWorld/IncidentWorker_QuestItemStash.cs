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
		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);

		private const float NoSitePartChance = 0.15f;

		private static readonly string ItemStashQuestThreatTag = "ItemStashQuestThreat";

		public IncidentWorker_QuestItemStash()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && (Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined) != null && TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1)) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, null, out faction, true, null);
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
			else if (!TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				result = false;
			}
			else if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.ItemStash, (!Rand.Chance(0.15f)) ? IncidentWorker_QuestItemStash.ItemStashQuestThreatTag : null, out sitePart, out siteFaction, null, true, null))
			{
				result = false;
			}
			else
			{
				int randomInRange = IncidentWorker_QuestItemStash.TimeoutDaysRange.RandomInRange;
				List<Thing> items = this.GenerateItems(siteFaction);
				Site site = IncidentWorker_QuestItemStash.CreateSite(tile, sitePart, randomInRange, siteFaction, items);
				string letterText = this.GetLetterText(faction, items, randomInRange, site, site.parts.FirstOrDefault<SitePart>());
				Find.LetterStack.ReceiveLetter(this.def.letterLabel, letterText, this.def.letterDef, site, faction, null);
				result = true;
			}
			return result;
		}

		protected virtual List<Thing> GenerateItems(Faction siteFaction)
		{
			return ThingSetMakerDefOf.Reward_ItemStashQuestContents.root.Generate();
		}

		public static Site CreateSite(int tile, SitePartDef sitePart, int days, Faction siteFaction, IList<Thing> items)
		{
			Site site = SiteMaker.MakeSite(SiteCoreDefOf.ItemStash, sitePart, tile, siteFaction, true);
			site.sitePartsKnown = true;
			site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
			site.GetComponent<ItemStashContentsComp>().contents.TryAddRangeOrTransfer(items, false, false);
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

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_QuestItemStash()
		{
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

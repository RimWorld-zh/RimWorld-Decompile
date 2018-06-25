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
		private const float ChanceToRevealSitePart = 0.5f;

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
				bool sitePartsKnown = Rand.Chance(0.5f);
				string letterText = this.GetLetterText(faction, items, randomInRange, sitePart, sitePartsKnown);
				Site o = IncidentWorker_QuestItemStash.CreateSite(tile, sitePart, randomInRange, siteFaction, items, sitePartsKnown);
				Find.LetterStack.ReceiveLetter(this.def.letterLabel, letterText, this.def.letterDef, o, faction, null);
				result = true;
			}
			return result;
		}

		private string GetSitePartInfo(SitePartDef sitePart, string leaderLabel)
		{
			string result;
			if (sitePart == null)
			{
				result = "ItemStashSitePart_Nothing".Translate(new object[]
				{
					leaderLabel
				});
			}
			else
			{
				result = "ItemStashSitePart_Part".Translate(new object[]
				{
					leaderLabel,
					string.Format(sitePart.descriptionDialogue, new object[0])
				});
			}
			return result;
		}

		protected virtual List<Thing> GenerateItems(Faction siteFaction)
		{
			return ThingSetMakerDefOf.Reward_ItemStashQuestContents.root.Generate();
		}

		public static Site CreateSite(int tile, SitePartDef sitePart, int days, Faction siteFaction, IList<Thing> items, bool sitePartsKnown)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.Tile = tile;
			site.core = SiteCoreDefOf.ItemStash;
			site.writeSiteParts = sitePartsKnown;
			if (sitePart != null)
			{
				site.parts.Add(sitePart);
			}
			site.SetFaction(siteFaction);
			site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
			site.GetComponent<ItemStashContentsComp>().contents.TryAddRangeOrTransfer(items, false, false);
			Find.WorldObjects.Add(site);
			return site;
		}

		private string GetLetterText(Faction alliedFaction, List<Thing> items, int days, SitePartDef sitePart, bool sitePartsKnown)
		{
			string text;
			if (sitePartsKnown)
			{
				text = this.GetSitePartInfo(sitePart, alliedFaction.leader.LabelShort).CapitalizeFirst();
			}
			else
			{
				text = "ItemStashSitePart_Unknown".Translate(new object[]
				{
					alliedFaction.leader.LabelShort
				}).CapitalizeFirst();
			}
			string text2 = string.Format(this.def.letterText, new object[]
			{
				alliedFaction.leader.LabelShort,
				alliedFaction.def.leaderTitle,
				alliedFaction.Name,
				GenLabel.ThingsLabel(items).TrimEndNewlines(),
				days.ToString(),
				text
			}).CapitalizeFirst();
			if (items.Count == 1 || (items.Count >= 2 && items.All((Thing x) => x.def == items[0].def)))
			{
				string text3 = text2;
				text2 = string.Concat(new string[]
				{
					text3,
					"\n\n---\n\n",
					items[0].LabelCap,
					": ",
					items[0].DescriptionFlavor
				});
			}
			return text2;
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

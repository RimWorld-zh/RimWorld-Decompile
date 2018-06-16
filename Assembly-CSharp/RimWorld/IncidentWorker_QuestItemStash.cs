using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000351 RID: 849
	public class IncidentWorker_QuestItemStash : IncidentWorker
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x0007BFF8 File Offset: 0x0007A3F8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && (Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined) != null && TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1)) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, null, out faction, true, null);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0007C058 File Offset: 0x0007A458
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

		// Token: 0x06000EAA RID: 3754 RVA: 0x0007C15C File Offset: 0x0007A55C
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

		// Token: 0x06000EAB RID: 3755 RVA: 0x0007C1BC File Offset: 0x0007A5BC
		protected virtual List<Thing> GenerateItems(Faction siteFaction)
		{
			return ThingSetMakerDefOf.Reward_ItemStashQuestContents.root.Generate();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0007C1E0 File Offset: 0x0007A5E0
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

		// Token: 0x06000EAD RID: 3757 RVA: 0x0007C26C File Offset: 0x0007A66C
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
			if (items.Count == 1)
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

		// Token: 0x04000902 RID: 2306
		private const float ChanceToRevealSitePart = 0.5f;

		// Token: 0x04000903 RID: 2307
		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 45);

		// Token: 0x04000904 RID: 2308
		private const float NoSitePartChance = 0.15f;

		// Token: 0x04000905 RID: 2309
		private static readonly string ItemStashQuestThreatTag = "ItemStashQuestThreat";
	}
}

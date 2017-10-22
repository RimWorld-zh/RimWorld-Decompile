using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_QuestItemStash : IncidentWorker
	{
		private const float ChanceToRevealSitePart = 0.5f;

		private static readonly IntRange TimeoutDaysRange = new IntRange(10, 30);

		private static readonly IntRange FeeRange = new IntRange(50, 500);

		private const int FeeDemandTimeoutTicks = 60000;

		private const float NoSitePartChance = 0.15f;

		private static readonly string ItemStashQuestThreatTag = "ItemStashQuestThreat";

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Faction faction = default(Faction);
			int num = default(int);
			return base.CanFireNowSub(target) && Find.FactionManager.RandomAlliedFaction(false, false, false, TechLevel.Undefined) != null && TileFinder.TryFindNewSiteTile(out num, 8, 30, false, true, -1) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, (IEnumerable<SitePartDef>)null, out faction, true, (Predicate<Faction>)null);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Faction faction = Find.FactionManager.RandomAlliedFaction(false, false, false, TechLevel.Undefined);
			bool result;
			int tile = default(int);
			SitePartDef sitePart = default(SitePartDef);
			Faction siteFaction = default(Faction);
			if (faction == null)
			{
				result = false;
			}
			else if (!TileFinder.TryFindNewSiteTile(out tile, 8, 30, false, true, -1))
			{
				result = false;
			}
			else if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteCoreDefOf.ItemStash, (!Rand.Chance(0.15f)) ? IncidentWorker_QuestItemStash.ItemStashQuestThreatTag : null, out sitePart, out siteFaction, (Faction)null, true, (Predicate<Faction>)null))
			{
				result = false;
			}
			else
			{
				int randomInRange = IncidentWorker_QuestItemStash.TimeoutDaysRange.RandomInRange;
				List<Thing> list = this.GenerateItems(siteFaction);
				bool sitePartsKnown = Rand.Chance(0.5f);
				int num = 0;
				if (Rand.Chance(this.FeeDemandChance(faction)))
				{
					num = IncidentWorker_QuestItemStash.FeeRange.RandomInRange;
				}
				string letterText = this.GetLetterText(faction, list, randomInRange, sitePart, sitePartsKnown, num);
				if (num > 0)
				{
					Map map = TradeUtility.PlayerHomeMapWithMostLaunchableSilver();
					ChoiceLetter_ItemStashFeeDemand choiceLetter_ItemStashFeeDemand = (ChoiceLetter_ItemStashFeeDemand)LetterMaker.MakeLetter(base.def.letterLabel, letterText, LetterDefOf.ItemStashFeeDemand);
					choiceLetter_ItemStashFeeDemand.title = "ItemStashQuestTitle".Translate();
					choiceLetter_ItemStashFeeDemand.radioMode = true;
					choiceLetter_ItemStashFeeDemand.map = map;
					choiceLetter_ItemStashFeeDemand.fee = num;
					choiceLetter_ItemStashFeeDemand.siteDaysTimeout = randomInRange;
					choiceLetter_ItemStashFeeDemand.items.TryAddRangeOrTransfer(list, false, false);
					choiceLetter_ItemStashFeeDemand.siteFaction = siteFaction;
					choiceLetter_ItemStashFeeDemand.sitePart = sitePart;
					choiceLetter_ItemStashFeeDemand.alliedFaction = faction;
					choiceLetter_ItemStashFeeDemand.sitePartsKnown = sitePartsKnown;
					choiceLetter_ItemStashFeeDemand.StartTimeout(60000);
					Find.LetterStack.ReceiveLetter(choiceLetter_ItemStashFeeDemand, (string)null);
				}
				else
				{
					Site o = IncidentWorker_QuestItemStash.CreateSite(tile, sitePart, randomInRange, siteFaction, list, sitePartsKnown);
					Find.LetterStack.ReceiveLetter(base.def.letterLabel, letterText, base.def.letterDef, (WorldObject)o, (string)null);
				}
				result = true;
			}
			return result;
		}

		private string GetSitePartInfo(SitePartDef sitePart, string leaderLabel)
		{
			return (sitePart != null) ? "ItemStashSitePart_Part".Translate(leaderLabel, string.Format(sitePart.descriptionDialogue)) : "ItemStashSitePart_Nothing".Translate(leaderLabel);
		}

		private List<Thing> GenerateItems(Faction siteFaction)
		{
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				techLevel = new TechLevel?((siteFaction == null) ? TechLevel.Spacer : siteFaction.def.techLevel)
			};
			return ItemCollectionGeneratorDefOf.ItemStashQuest.Worker.Generate(parms);
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
			((WorldObject)site).GetComponent<TimeoutComp>().StartTimeout(days * 60000);
			((WorldObject)site).GetComponent<ItemStashContentsComp>().contents.TryAddRangeOrTransfer(items, false, false);
			Find.WorldObjects.Add(site);
			return site;
		}

		private string GetLetterText(Faction alliedFaction, List<Thing> items, int days, SitePartDef sitePart, bool sitePartsKnown, int fee)
		{
			string text = (!sitePartsKnown) ? "ItemStashSitePart_Unknown".Translate(alliedFaction.leader.LabelShort).CapitalizeFirst() : this.GetSitePartInfo(sitePart, alliedFaction.leader.LabelShort).CapitalizeFirst();
			string text2 = string.Format(base.def.letterText, alliedFaction.leader.LabelShort, alliedFaction.def.leaderTitle, alliedFaction.Name, GenLabel.ThingsLabel(items).TrimEndNewlines(), days.ToString(), text).CapitalizeFirst();
			if (fee > 0)
			{
				text2 = text2 + "\n\n" + "ItemStashQuestFeeDemand".Translate(alliedFaction.leader.LabelShort, fee).CapitalizeFirst();
			}
			if (items.Count == 1)
			{
				string text3 = text2;
				text2 = text3 + "\n\n---\n\n" + items[0].LabelCap + ": " + items[0].GetDescription();
			}
			return text2;
		}

		private float FeeDemandChance(Faction faction)
		{
			FactionRelation factionRelation = faction.RelationWith(Faction.OfPlayer, false);
			float x = Mathf.Max(factionRelation.goodwill, 0f);
			return GenMath.LerpDouble(0f, 100f, 0.5f, 0f, x);
		}
	}
}

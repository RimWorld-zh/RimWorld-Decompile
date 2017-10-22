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

		private const float AIPersonaCoreConsiderChance = 0.25f;

		private const int FeeDemandTimeoutTicks = 60000;

		private static readonly IntRange TimeoutDaysRange = new IntRange(10, 30);

		private static readonly IntRange ThingsCountRange = new IntRange(5, 9);

		private static readonly FloatRange TotalMarketValueRange = new FloatRange(2000f, 3000f);

		private static readonly IntRange FeeRange = new IntRange(50, 500);

		private static readonly IntRange NeurotrainersCountRange = new IntRange(3, 5);

		private List<Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>> possibleItemCollectionGenerators = new List<Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>>();

		private List<SitePartDef> possibleSitePartsInt = new List<SitePartDef>();

		private List<SitePartDef> PossibleSiteParts
		{
			get
			{
				this.possibleSitePartsInt.Clear();
				this.possibleSitePartsInt.Add(SitePartDefOf.Manhunters);
				this.possibleSitePartsInt.Add(SitePartDefOf.Outpost);
				this.possibleSitePartsInt.Add(SitePartDefOf.Turrets);
				this.possibleSitePartsInt.Add(null);
				return this.possibleSitePartsInt;
			}
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			SitePartDef sitePartDef = default(SitePartDef);
			if (!SiteMaker.TryFindNewRandomSitePartFor(SiteCoreDefOf.ItemStash, (IEnumerable<SitePartDef>)null, (IEnumerable<SitePartDef>)this.PossibleSiteParts, (Faction)null, out sitePartDef, true, (Predicate<Faction>)null))
			{
				return false;
			}
			object obj;
			if (sitePartDef != null)
			{
				IEnumerable<SitePartDef> enumerable = Gen.YieldSingle(sitePartDef);
				obj = enumerable;
			}
			else
			{
				obj = null;
			}
			IEnumerable<SitePartDef> parts = (IEnumerable<SitePartDef>)obj;
			int num = default(int);
			Faction faction = default(Faction);
			return Find.FactionManager.RandomAlliedFaction(false, false, false) != null && TileFinder.TryFindNewSiteTile(out num) && SiteMaker.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, parts, out faction, true, (Predicate<Faction>)null);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Faction faction = Find.FactionManager.RandomAlliedFaction(false, false, false);
			if (faction == null)
			{
				return false;
			}
			int tile = default(int);
			if (!TileFinder.TryFindNewSiteTile(out tile))
			{
				return false;
			}
			SitePartDef sitePartDef = default(SitePartDef);
			if (!SiteMaker.TryFindNewRandomSitePartFor(SiteCoreDefOf.ItemStash, (IEnumerable<SitePartDef>)null, (IEnumerable<SitePartDef>)this.PossibleSiteParts, (Faction)null, out sitePartDef, true, (Predicate<Faction>)null))
			{
				return false;
			}
			object obj;
			if (sitePartDef != null)
			{
				IEnumerable<SitePartDef> enumerable = Gen.YieldSingle(sitePartDef);
				obj = enumerable;
			}
			else
			{
				obj = null;
			}
			IEnumerable<SitePartDef> parts = (IEnumerable<SitePartDef>)obj;
			Faction siteFaction = default(Faction);
			if (!SiteMaker.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, parts, out siteFaction, true, (Predicate<Faction>)null))
			{
				return false;
			}
			int randomInRange = IncidentWorker_QuestItemStash.TimeoutDaysRange.RandomInRange;
			List<Thing> list = this.GenerateItems(siteFaction);
			bool sitePartsKnown = Rand.Chance(0.5f);
			string letterText = this.GetLetterText(faction, list, randomInRange, sitePartDef, sitePartsKnown);
			if (Rand.Chance(this.FeeDemandChance(faction)))
			{
				Map map = TradeUtility.PlayerHomeMapWithMostLaunchableSilver();
				int randomInRange2 = IncidentWorker_QuestItemStash.FeeRange.RandomInRange;
				string text = letterText + "\n\n" + "ItemStashQuestFeeDemand".Translate(faction.leader.LabelShort, randomInRange2).CapitalizeFirst();
				ChoiceLetter_ItemStashFeeDemand choiceLetter_ItemStashFeeDemand = (ChoiceLetter_ItemStashFeeDemand)LetterMaker.MakeLetter(base.def.letterLabel, text, LetterDefOf.ItemStashFeeDemand);
				choiceLetter_ItemStashFeeDemand.title = "ItemStashQuestTitle".Translate();
				choiceLetter_ItemStashFeeDemand.radioMode = true;
				choiceLetter_ItemStashFeeDemand.map = map;
				choiceLetter_ItemStashFeeDemand.fee = randomInRange2;
				choiceLetter_ItemStashFeeDemand.siteDaysTimeout = randomInRange;
				choiceLetter_ItemStashFeeDemand.items.TryAddRange(list, false);
				choiceLetter_ItemStashFeeDemand.siteFaction = siteFaction;
				choiceLetter_ItemStashFeeDemand.sitePart = sitePartDef;
				choiceLetter_ItemStashFeeDemand.alliedFaction = faction;
				choiceLetter_ItemStashFeeDemand.sitePartsKnown = sitePartsKnown;
				choiceLetter_ItemStashFeeDemand.StartTimeout(60000);
				Find.LetterStack.ReceiveLetter(choiceLetter_ItemStashFeeDemand, (string)null);
			}
			else
			{
				Site o = IncidentWorker_QuestItemStash.CreateSite(tile, sitePartDef, randomInRange, siteFaction, list, sitePartsKnown);
				Find.LetterStack.ReceiveLetter(base.def.letterLabel, letterText, base.def.letterDef, (WorldObject)o, (string)null);
			}
			return true;
		}

		private string GetSitePartInfoKey(SitePartDef sitePart)
		{
			if (sitePart == null)
			{
				return "ItemStashSitePart_Nothing";
			}
			if (sitePart == SitePartDefOf.Manhunters)
			{
				return "ItemStashSitePart_Manhunters";
			}
			if (sitePart == SitePartDefOf.Outpost)
			{
				return "ItemStashSitePart_Outpost";
			}
			if (sitePart == SitePartDefOf.Turrets)
			{
				return "ItemStashSitePart_Turrets";
			}
			throw new InvalidOperationException("Site part not handled: " + sitePart);
		}

		private List<Thing> GenerateItems(Faction siteFaction)
		{
			this.CalculatePossibleItemCollectionGenerators(siteFaction);
			Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams> pair = this.possibleItemCollectionGenerators.RandomElement();
			return pair.First.Worker.Generate(pair.Second);
		}

		private void CalculatePossibleItemCollectionGenerators(Faction siteFaction)
		{
			TechLevel techLevel = (siteFaction == null) ? TechLevel.Spacer : siteFaction.def.techLevel;
			this.possibleItemCollectionGenerators.Clear();
			if (Rand.Chance(0.25f) && (int)techLevel >= (int)ThingDefOf.AIPersonaCore.techLevel)
			{
				ItemCollectionGeneratorDef aIPersonaCores = ItemCollectionGeneratorDefOf.AIPersonaCores;
				ItemCollectionGeneratorParams second = new ItemCollectionGeneratorParams
				{
					count = 1
				};
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(aIPersonaCores, second));
			}
			if ((int)techLevel >= (int)ThingDefOf.Neurotrainer.techLevel)
			{
				ItemCollectionGeneratorDef neurotrainers = ItemCollectionGeneratorDefOf.Neurotrainers;
				ItemCollectionGeneratorParams second2 = new ItemCollectionGeneratorParams
				{
					count = IncidentWorker_QuestItemStash.NeurotrainersCountRange.RandomInRange
				};
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(neurotrainers, second2));
			}
			ItemCollectionGeneratorParams second3 = new ItemCollectionGeneratorParams
			{
				count = IncidentWorker_QuestItemStash.ThingsCountRange.RandomInRange,
				totalMarketValue = IncidentWorker_QuestItemStash.TotalMarketValueRange.RandomInRange,
				techLevel = techLevel
			};
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Weapons, second3));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.RawResources, second3));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Apparel, second3));
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
			((WorldObject)site).GetComponent<ItemStashContentsComp>().contents.TryAddRange(items, false);
			Find.WorldObjects.Add(site);
			return site;
		}

		private string GetLetterText(Faction alliedFaction, List<Thing> items, int days, SitePartDef sitePart, bool sitePartsKnown)
		{
			string text = (!sitePartsKnown) ? "ItemStashSitePart_Unknown".Translate(alliedFaction.leader.LabelShort).CapitalizeFirst() : this.GetSitePartInfoKey(sitePart).Translate(alliedFaction.leader.LabelShort).CapitalizeFirst();
			return string.Format(base.def.letterText, alliedFaction.leader.LabelShort, alliedFaction.def.leaderTitle, alliedFaction.Name, GenLabel.ThingsLabel(items).TrimEndNewlines(), days.ToString(), text).CapitalizeFirst();
		}

		private float FeeDemandChance(Faction faction)
		{
			FactionRelation factionRelation = faction.RelationWith(Faction.OfPlayer, false);
			float x = Mathf.Max(factionRelation.goodwill, 0f);
			return GenMath.LerpDouble(0f, 100f, 0.5f, 0f, x);
		}
	}
}

using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_ItemStashQuest : ItemCollectionGenerator
	{
		private List<Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>> possibleItemCollectionGenerators = new List<Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>>();

		private static readonly IntRange ThingsCountRange = new IntRange(5, 9);

		private static readonly FloatRange TotalMarketValueRange = new FloatRange(2000f, 3000f);

		private static readonly IntRange NeurotrainersCountRange = new IntRange(3, 5);

		private const float AIPersonaCoreExtraChance = 0.25f;

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (!techLevel.HasValue) ? TechLevel.Spacer : techLevel.Value;
			this.CalculatePossibleItemCollectionGenerators(techLevel2);
			if (this.possibleItemCollectionGenerators.Any())
			{
				Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams> pair = this.possibleItemCollectionGenerators.RandomElement();
				outThings.AddRange(pair.First.Worker.Generate(pair.Second));
			}
		}

		private void CalculatePossibleItemCollectionGenerators(TechLevel techLevel)
		{
			this.possibleItemCollectionGenerators.Clear();
			if ((int)techLevel >= (int)ThingDefOf.AIPersonaCore.techLevel)
			{
				ItemCollectionGeneratorDef standard = ItemCollectionGeneratorDefOf.Standard;
				ItemCollectionGeneratorParams second = default(ItemCollectionGeneratorParams);
				second.extraAllowedDefs = Gen.YieldSingle(ThingDefOf.AIPersonaCore);
				second.count = 1;
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard, second));
				if (Rand.Chance(0.25f) && !this.PlayerOrItemStashHasAIPersonaCore())
					return;
			}
			if ((int)techLevel >= (int)ThingDefOf.MechSerumNeurotrainer.techLevel)
			{
				ItemCollectionGeneratorDef standard2 = ItemCollectionGeneratorDefOf.Standard;
				ItemCollectionGeneratorParams second2 = default(ItemCollectionGeneratorParams);
				second2.extraAllowedDefs = Gen.YieldSingle(ThingDefOf.MechSerumNeurotrainer);
				second2.count = ItemCollectionGenerator_ItemStashQuest.NeurotrainersCountRange.RandomInRange;
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard2, second2));
			}
			List<ThingDef> allGeneratableItems = ItemCollectionGeneratorUtility.allGeneratableItems;
			for (int i = 0; i < allGeneratableItems.Count; i++)
			{
				ThingDef thingDef = allGeneratableItems[i];
				if ((int)techLevel >= (int)thingDef.techLevel && thingDef.itemGeneratorTags != null && thingDef.itemGeneratorTags.Contains(ItemCollectionGeneratorUtility.SpecialRewardTag))
				{
					ItemCollectionGeneratorDef standard3 = ItemCollectionGeneratorDefOf.Standard;
					ItemCollectionGeneratorParams second3 = default(ItemCollectionGeneratorParams);
					second3.extraAllowedDefs = Gen.YieldSingle(thingDef);
					second3.count = 1;
					this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard3, second3));
				}
			}
			ItemCollectionGeneratorParams second4 = default(ItemCollectionGeneratorParams);
			second4.count = ItemCollectionGenerator_ItemStashQuest.ThingsCountRange.RandomInRange;
			second4.totalMarketValue = ItemCollectionGenerator_ItemStashQuest.TotalMarketValueRange.RandomInRange;
			second4.techLevel = techLevel;
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Weapons, second4));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.RawResources, second4));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Apparel, second4));
		}

		private bool PlayerOrItemStashHasAIPersonaCore()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].listerThings.ThingsOfDef(ThingDefOf.AIPersonaCore).Count > 0)
				{
					return true;
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int j = 0; j < caravans.Count; j++)
			{
				if (caravans[j].IsPlayerControlled && CaravanInventoryUtility.HasThings(caravans[j], ThingDefOf.AIPersonaCore, 1, null))
				{
					return true;
				}
			}
			List<Site> sites = Find.WorldObjects.Sites;
			for (int k = 0; k < sites.Count; k++)
			{
				ItemStashContentsComp component = ((WorldObject)sites[k]).GetComponent<ItemStashContentsComp>();
				if (component != null)
				{
					ThingOwner contents = component.contents;
					for (int l = 0; l < contents.Count; l++)
					{
						if (contents[l].def == ThingDefOf.AIPersonaCore)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}

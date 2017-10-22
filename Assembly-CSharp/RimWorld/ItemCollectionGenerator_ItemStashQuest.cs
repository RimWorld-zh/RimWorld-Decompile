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
				ItemCollectionGeneratorParams second = new ItemCollectionGeneratorParams
				{
					extraAllowedDefs = Gen.YieldSingle(ThingDefOf.AIPersonaCore),
					count = new int?(1)
				};
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard, second));
				if (Rand.Chance(0.25f) && !this.PlayerOrItemStashHasAIPersonaCore())
					return;
			}
			if ((int)techLevel >= (int)ThingDefOf.MechSerumNeurotrainer.techLevel)
			{
				ItemCollectionGeneratorDef standard2 = ItemCollectionGeneratorDefOf.Standard;
				ItemCollectionGeneratorParams second2 = new ItemCollectionGeneratorParams
				{
					extraAllowedDefs = Gen.YieldSingle(ThingDefOf.MechSerumNeurotrainer),
					count = new int?(ItemCollectionGenerator_ItemStashQuest.NeurotrainersCountRange.RandomInRange)
				};
				this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard2, second2));
			}
			List<ThingDef> allGeneratableItems = ItemCollectionGeneratorUtility.allGeneratableItems;
			for (int i = 0; i < allGeneratableItems.Count; i++)
			{
				ThingDef thingDef = allGeneratableItems[i];
				if ((int)techLevel >= (int)thingDef.techLevel && thingDef.itemGeneratorTags != null && thingDef.itemGeneratorTags.Contains(ItemCollectionGeneratorUtility.SpecialRewardTag))
				{
					ItemCollectionGeneratorDef standard3 = ItemCollectionGeneratorDefOf.Standard;
					ItemCollectionGeneratorParams second3 = new ItemCollectionGeneratorParams
					{
						extraAllowedDefs = Gen.YieldSingle(thingDef),
						count = new int?(1)
					};
					this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(standard3, second3));
				}
			}
			ItemCollectionGeneratorParams second4 = new ItemCollectionGeneratorParams
			{
				count = new int?(ItemCollectionGenerator_ItemStashQuest.ThingsCountRange.RandomInRange),
				totalMarketValue = new float?(ItemCollectionGenerator_ItemStashQuest.TotalMarketValueRange.RandomInRange),
				techLevel = new TechLevel?(techLevel)
			};
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Weapons, second4));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.RawResources, second4));
			this.possibleItemCollectionGenerators.Add(new Pair<ItemCollectionGeneratorDef, ItemCollectionGeneratorParams>(ItemCollectionGeneratorDefOf.Apparel, second4));
		}

		private bool PlayerOrItemStashHasAIPersonaCore()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < maps.Count)
				{
					if (maps[num].listerThings.ThingsOfDef(ThingDefOf.AIPersonaCore).Count > 0)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && CaravanInventoryUtility.HasThings(caravans[i], ThingDefOf.AIPersonaCore, 1, null))
						goto IL_0087;
				}
				List<Site> sites = Find.WorldObjects.Sites;
				for (int j = 0; j < sites.Count; j++)
				{
					ItemStashContentsComp component = ((WorldObject)sites[j]).GetComponent<ItemStashContentsComp>();
					if (component != null)
					{
						ThingOwner contents = component.contents;
						for (int k = 0; k < contents.Count; k++)
						{
							if (contents[k].def == ThingDefOf.AIPersonaCore)
								goto IL_00fe;
						}
					}
				}
				result = false;
				break;
				IL_00fe:
				result = true;
				break;
				IL_0087:
				result = true;
				break;
			}
			return result;
		}
	}
}

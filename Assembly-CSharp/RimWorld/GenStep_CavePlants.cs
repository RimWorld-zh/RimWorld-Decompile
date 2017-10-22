using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_CavePlants : GenStep
	{
		private const float PlantMinGrowth = 0.07f;

		private const float PlantChancePerCell = 0.18f;

		public override void Generate(Map map)
		{
			map.regionAndRoomUpdater.Enabled = false;
			MapGenFloatGrid caves = MapGenerator.Caves;
			List<ThingDef> source = (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant
			select x).ToList();
			foreach (IntVec3 item in map.AllCells.InRandomOrder(null))
			{
				if (item.GetEdifice(map) == null && item.GetCover(map) == null && !(caves[item] <= 0.0) && item.Roofed(map) && !(map.fertilityGrid.FertilityAt(item) <= 0.0) && Rand.Chance(0.18f))
				{
					IEnumerable<ThingDef> source2 = from def in source
					where def.CanEverPlantAt(item, map)
					select def;
					if (source2.Any())
					{
						ThingDef thingDef = source2.RandomElement();
						int randomInRange = thingDef.plant.wildClusterSizeRange.RandomInRange;
						for (int num = 0; num < randomInRange; num++)
						{
							IntVec3 loc = default(IntVec3);
							if (num == 0)
							{
								loc = item;
							}
							else if (!GenPlantReproduction.TryFindReproductionDestination(item, thingDef, SeedTargFindMode.MapGenCluster, map, out loc))
								break;
							Plant plant = (Plant)ThingMaker.MakeThing(thingDef, null);
							plant.Growth = Rand.Range(0.07f, 1f);
							if (plant.def.plant.LimitedLifespan)
							{
								plant.Age = Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 50, 0));
							}
							GenSpawn.Spawn(plant, loc, map);
						}
					}
				}
			}
			map.regionAndRoomUpdater.Enabled = true;
		}
	}
}

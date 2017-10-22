using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenPlant
	{
		public static bool GrowthSeasonNow(IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			bool result;
			if (roomOrAdjacent == null)
			{
				result = false;
			}
			else if (roomOrAdjacent.UsesOutdoorTemperature)
			{
				result = map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow;
			}
			else
			{
				float temperature = c.GetTemperature(map);
				result = (temperature > 0.0 && temperature < 58.0);
			}
			return result;
		}

		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.20000000298023224;
		}

		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map)
		{
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + plantDef + " which is not a plant.");
			}
			bool result;
			if (!c.InBounds(map))
			{
				result = false;
			}
			else if (map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
			{
				result = false;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.def.BlockPlanting)
						goto IL_0085;
					if (plantDef.passability == Traversability.Impassable && (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Plant))
					{
						goto IL_00dd;
					}
				}
				if (plantDef.passability == Traversability.Impassable)
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = c + GenAdj.CardinalDirections[j];
						if (c2.InBounds(map))
						{
							Building edifice = c2.GetEdifice(map);
							if (edifice != null && edifice.def.IsDoor)
								goto IL_0155;
						}
					}
				}
				result = true;
			}
			goto IL_0174;
			IL_0155:
			result = false;
			goto IL_0174;
			IL_0085:
			result = false;
			goto IL_0174;
			IL_0174:
			return result;
			IL_00dd:
			result = false;
			goto IL_0174;
		}

		public static void LogPlantProportions()
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			foreach (ThingDef allWildPlant in Find.VisibleMap.Biome.AllWildPlants)
			{
				dictionary.Add(allWildPlant, 0f);
			}
			float num = 0f;
			foreach (IntVec3 allCell in Find.VisibleMap.AllCells)
			{
				Plant plant = allCell.GetPlant(Find.VisibleMap);
				if (plant != null && dictionary.ContainsKey(plant.def))
				{
					Dictionary<ThingDef, float> dictionary2;
					ThingDef def;
					(dictionary2 = dictionary)[def = plant.def] = (float)(dictionary2[def] + 1.0);
					num = (float)(num + 1.0);
				}
			}
			foreach (ThingDef allWildPlant2 in Find.VisibleMap.Biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2;
				ThingDef key;
				(dictionary2 = dictionary)[key = allWildPlant2] = dictionary2[key] / num;
			}
			Dictionary<ThingDef, float> dictionary3 = GenPlant.CalculateDesiredPlantProportions(Find.VisibleMap.Biome);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PLANT           EXPECTED             FOUND");
			foreach (ThingDef allWildPlant3 in Find.VisibleMap.Biome.AllWildPlants)
			{
				stringBuilder.AppendLine(allWildPlant3.LabelCap + "       " + dictionary3[allWildPlant3].ToStringPercent() + "        " + dictionary[allWildPlant3].ToStringPercent());
			}
			Log.Message(stringBuilder.ToString());
		}

		public static Dictionary<ThingDef, float> CalculateDesiredPlantProportions(BiomeDef biome)
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			float num = 0f;
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.plant != null)
				{
					float num2 = biome.CommonalityOfPlant(allDef);
					dictionary.Add(allDef, num2);
					num += num2;
				}
			}
			foreach (ThingDef allWildPlant in biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2;
				ThingDef key;
				(dictionary2 = dictionary)[key = allWildPlant] = dictionary2[key] / num;
			}
			return dictionary;
		}

		public static IEnumerable<ThingDef> ValidPlantTypesForGrowers(List<IPlantToGrowSettable> sel)
		{
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant
			select def).GetEnumerator())
			{
				ThingDef plantDef;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						_003CValidPlantTypesForGrowers_003Ec__Iterator0 _003CValidPlantTypesForGrowers_003Ec__Iterator = (_003CValidPlantTypesForGrowers_003Ec__Iterator0)/*Error near IL_007c: stateMachine*/;
						plantDef = enumerator.Current;
						if (sel.TrueForAll((Predicate<IPlantToGrowSettable>)((IPlantToGrowSettable x) => GenPlant.CanSowOnGrower(plantDef, x))))
							break;
						continue;
					}
					yield break;
				}
				yield return plantDef;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_011b:
			/*Error near IL_011c: Unexpected return in MoveNext()*/;
		}

		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			bool result;
			if (obj is Zone)
			{
				result = plantDef.plant.sowTags.Contains("Ground");
			}
			else
			{
				Thing thing = obj as Thing;
				result = (thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag));
			}
			return result;
		}

		public static Thing AdjacentSowBlocker(ThingDef plantDef, IntVec3 c, Map map)
		{
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < 8)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[num];
					if (c2.InBounds(map))
					{
						Plant plant = c2.GetPlant(map);
						if (plant != null && (plant.def.plant.blockAdjacentSow || (plantDef.plant.blockAdjacentSow && plant.sown)))
						{
							result = plant;
							break;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		internal static void LogPlantData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All plant data");
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.plant != null)
				{
					float num = (float)(allDef.plant.growDays * 2.0);
					float num2 = (float)(allDef.plant.lifespanFraction / (allDef.plant.lifespanFraction - 1.0));
					float num3 = num2 * num;
					float num4 = (float)((num3 + num * 0.39999997615814209) / allDef.plant.reproduceMtbDays);
					stringBuilder.AppendLine(allDef.defName);
					stringBuilder.AppendLine("  lifeSpanDays:\t\t\t\t" + allDef.plant.LifespanDays.ToString("F2"));
					stringBuilder.AppendLine("  daysToGrown:\t\t\t\t" + allDef.plant.growDays);
					stringBuilder.AppendLine("  guess days to grown:\t\t" + num.ToString("F2"));
					stringBuilder.AppendLine("  grown days before death:\t" + num3.ToString("F2"));
					stringBuilder.AppendLine("  percent of life grown:\t" + num2.ToStringPercent());
					if (allDef.plant.reproduces)
					{
						stringBuilder.AppendLine("  MTB seed emits (days):\t" + allDef.plant.reproduceMtbDays.ToString("F2"));
						stringBuilder.AppendLine("  average seeds emitted:\t" + num4.ToString("F2"));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min((float)(255.0 * plant.def.plant.topWindExposure), 255f);
		}

		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = GenPlant.GetWindExposure(plant));
			colors[0].a = (colors[3].a = (byte)0);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public static class PlantUtility
	{
		public static bool GrowthSeasonNow(IntVec3 c, Map map, bool forSowing = false)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent == null)
			{
				return false;
			}
			if (!roomOrAdjacent.UsesOutdoorTemperature)
			{
				float temperature = c.GetTemperature(map);
				return temperature > 0f && temperature < 58f;
			}
			if (forSowing)
			{
				return map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNowForSowing;
			}
			return map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow;
		}

		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map)
		{
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + plantDef + " which is not a plant.", false);
			}
			if (!c.InBounds(map))
			{
				return false;
			}
			if (map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.BlockPlanting)
				{
					return false;
				}
				if (plantDef.passability == Traversability.Impassable && (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Plant))
				{
					return false;
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
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public static void LogPlantProportions()
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			foreach (ThingDef key in Find.CurrentMap.Biome.AllWildPlants)
			{
				dictionary.Add(key, 0f);
			}
			float num = 0f;
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Plant plant = c.GetPlant(Find.CurrentMap);
				if (plant != null && dictionary.ContainsKey(plant.def))
				{
					Dictionary<ThingDef, float> dictionary2;
					ThingDef def;
					(dictionary2 = dictionary)[def = plant.def] = dictionary2[def] + 1f;
					num += 1f;
				}
			}
			foreach (ThingDef thingDef in Find.CurrentMap.Biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2;
				ThingDef key2;
				(dictionary2 = dictionary)[key2 = thingDef] = dictionary2[key2] / num;
			}
			Dictionary<ThingDef, float> dictionary3 = PlantUtility.CalculateDesiredPlantProportions(Find.CurrentMap.Biome);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PLANT           EXPECTED             FOUND");
			foreach (ThingDef thingDef2 in Find.CurrentMap.Biome.AllWildPlants)
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					thingDef2.LabelCap,
					"       ",
					dictionary3[thingDef2].ToStringPercent(),
					"        ",
					dictionary[thingDef2].ToStringPercent()
				}));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		private static Dictionary<ThingDef, float> CalculateDesiredPlantProportions(BiomeDef biome)
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			float num = 0f;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null)
				{
					float num2 = biome.CommonalityOfPlant(thingDef);
					dictionary.Add(thingDef, num2);
					num += num2;
				}
			}
			foreach (ThingDef thingDef2 in biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2;
				ThingDef key;
				(dictionary2 = dictionary)[key = thingDef2] = dictionary2[key] / num;
			}
			return dictionary;
		}

		public static IEnumerable<ThingDef> ValidPlantTypesForGrowers(List<IPlantToGrowSettable> sel)
		{
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef plantDef = enumerator.Current;
					if (sel.TrueForAll((IPlantToGrowSettable x) => PlantUtility.CanSowOnGrower(plantDef, x)))
					{
						yield return plantDef;
					}
				}
			}
			yield break;
		}

		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			if (obj is Zone)
			{
				return plantDef.plant.sowTags.Contains("Ground");
			}
			Thing thing = obj as Thing;
			return thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag);
		}

		public static Thing AdjacentSowBlocker(ThingDef plantDef, IntVec3 c, Map map)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Plant plant = c2.GetPlant(map);
					if (plant != null && (plant.def.plant.blockAdjacentSow || (plantDef.plant.blockAdjacentSow && plant.sown)))
					{
						return plant;
					}
				}
			}
			return null;
		}

		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = PlantUtility.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		public static void LogFallColorForYear()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Fall color amounts for each latitude and each day of the year");
			stringBuilder.AppendLine("---------------------------------------");
			stringBuilder.Append("Lat".PadRight(6));
			for (int i = -90; i <= 90; i += 10)
			{
				stringBuilder.Append((i.ToString() + "d").PadRight(6));
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < 60; j += 5)
			{
				stringBuilder.Append(j.ToString().PadRight(6));
				for (int k = -90; k <= 90; k += 10)
				{
					stringBuilder.Append(PlantFallColors.GetFallColorFactor((float)k, j).ToString("F3").PadRight(6));
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[CompilerGenerated]
		private sealed class <ValidPlantTypesForGrowers>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal IEnumerator<ThingDef> $locvar0;

			internal List<IPlantToGrowSettable> sel;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private PlantUtility.<ValidPlantTypesForGrowers>c__Iterator0.<ValidPlantTypesForGrowers>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <ValidPlantTypesForGrowers>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = (from def in DefDatabase<ThingDef>.AllDefs
					where def.category == ThingCategory.Plant
					select def).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						ThingDef plantDef = enumerator.Current;
						if (sel.TrueForAll((IPlantToGrowSettable x) => PlantUtility.CanSowOnGrower(plantDef, x)))
						{
							this.$current = plantDef;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PlantUtility.<ValidPlantTypesForGrowers>c__Iterator0 <ValidPlantTypesForGrowers>c__Iterator = new PlantUtility.<ValidPlantTypesForGrowers>c__Iterator0();
				<ValidPlantTypesForGrowers>c__Iterator.sel = sel;
				return <ValidPlantTypesForGrowers>c__Iterator;
			}

			private static bool <>m__0(ThingDef def)
			{
				return def.category == ThingCategory.Plant;
			}

			private sealed class <ValidPlantTypesForGrowers>c__AnonStorey1
			{
				internal ThingDef plantDef;

				internal PlantUtility.<ValidPlantTypesForGrowers>c__Iterator0 <>f__ref$0;

				public <ValidPlantTypesForGrowers>c__AnonStorey1()
				{
				}

				internal bool <>m__0(IPlantToGrowSettable x)
				{
					return PlantUtility.CanSowOnGrower(this.plantDef, x);
				}
			}
		}
	}
}

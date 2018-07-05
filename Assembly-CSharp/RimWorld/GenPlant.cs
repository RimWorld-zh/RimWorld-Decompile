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
	public static class GenPlant
	{
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;

		public static bool GrowthSeasonNow(IntVec3 c, Map map, bool forSowing = false)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			bool result;
			if (roomOrAdjacent == null)
			{
				result = false;
			}
			else if (roomOrAdjacent.UsesOutdoorTemperature)
			{
				if (forSowing)
				{
					result = map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNowForSowing;
				}
				else
				{
					result = map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow;
				}
			}
			else
			{
				float temperature = c.GetTemperature(map);
				result = (temperature > 0f && temperature < 58f);
			}
			return result;
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
					{
						return false;
					}
					if (plantDef.passability == Traversability.Impassable)
					{
						if (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Plant)
						{
							return false;
						}
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
				result = true;
			}
			return result;
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
			Dictionary<ThingDef, float> dictionary3 = GenPlant.CalculateDesiredPlantProportions(Find.CurrentMap.Biome);
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
					if (sel.TrueForAll((IPlantToGrowSettable x) => GenPlant.CanSowOnGrower(plantDef, x)))
					{
						yield return plantDef;
					}
				}
			}
			yield break;
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
			colors[1].a = (colors[2].a = GenPlant.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, GenPlant.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(GenPlant.FallColorBegin, GenPlant.FallColorEnd, 0f, 1f, x);
		}

		public static void SetFallShaderGlobals(Map map)
		{
			if (GenPlant.FallIntensityOverride)
			{
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, GenPlant.FallIntensity);
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, GenPlant.GetFallColorFactor(vector.y, GenLocalDate.DayOfYear(map)));
			}
			Shader.SetGlobalInt("_FallGlobalControls", (!GenPlant.FallGlobalControls) ? 0 : 1);
			if (GenPlant.FallGlobalControls)
			{
				Shader.SetGlobalVector("_FallSrc", new Vector3(GenPlant.FallSrcR, GenPlant.FallSrcG, GenPlant.FallSrcB));
				Shader.SetGlobalVector("_FallDst", new Vector3(GenPlant.FallDstR, GenPlant.FallDstG, GenPlant.FallDstB));
				Shader.SetGlobalVector("_FallRange", new Vector3(GenPlant.FallRangeBegin, GenPlant.FallRangeEnd));
			}
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
					stringBuilder.Append(GenPlant.GetFallColorFactor((float)k, j).ToString("F3").PadRight(6));
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenPlant()
		{
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

			private GenPlant.<ValidPlantTypesForGrowers>c__Iterator0.<ValidPlantTypesForGrowers>c__AnonStorey1 $locvar1;

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
					case 1u:
						IL_E1:
						break;
					}
					if (enumerator.MoveNext())
					{
						ThingDef plantDef = enumerator.Current;
						if (sel.TrueForAll((IPlantToGrowSettable x) => GenPlant.CanSowOnGrower(plantDef, x)))
						{
							this.$current = plantDef;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_E1;
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
				GenPlant.<ValidPlantTypesForGrowers>c__Iterator0 <ValidPlantTypesForGrowers>c__Iterator = new GenPlant.<ValidPlantTypesForGrowers>c__Iterator0();
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

				internal GenPlant.<ValidPlantTypesForGrowers>c__Iterator0 <>f__ref$0;

				public <ValidPlantTypesForGrowers>c__AnonStorey1()
				{
				}

				internal bool <>m__0(IPlantToGrowSettable x)
				{
					return GenPlant.CanSowOnGrower(this.plantDef, x);
				}
			}
		}
	}
}

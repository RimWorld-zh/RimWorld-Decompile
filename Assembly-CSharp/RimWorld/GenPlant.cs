using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097E RID: 2430
	[HasDebugOutput]
	public static class GenPlant
	{
		// Token: 0x0400234E RID: 9038
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x0400234F RID: 9039
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x04002350 RID: 9040
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x04002351 RID: 9041
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x04002352 RID: 9042
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x04002353 RID: 9043
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x04002354 RID: 9044
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x04002355 RID: 9045
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x04002356 RID: 9046
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x04002357 RID: 9047
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x04002358 RID: 9048
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x04002359 RID: 9049
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x0400235A RID: 9050
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x0400235B RID: 9051
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;

		// Token: 0x060036B9 RID: 14009 RVA: 0x001D35F4 File Offset: 0x001D19F4
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

		// Token: 0x060036BA RID: 14010 RVA: 0x001D3680 File Offset: 0x001D1A80
		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x001D36A4 File Offset: 0x001D1AA4
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

		// Token: 0x060036BC RID: 14012 RVA: 0x001D3828 File Offset: 0x001D1C28
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

		// Token: 0x060036BD RID: 14013 RVA: 0x001D3A80 File Offset: 0x001D1E80
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

		// Token: 0x060036BE RID: 14014 RVA: 0x001D3B74 File Offset: 0x001D1F74
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

		// Token: 0x060036BF RID: 14015 RVA: 0x001D3BA0 File Offset: 0x001D1FA0
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

		// Token: 0x060036C0 RID: 14016 RVA: 0x001D3C20 File Offset: 0x001D2020
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

		// Token: 0x060036C1 RID: 14017 RVA: 0x001D3CB8 File Offset: 0x001D20B8
		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x001D3CF0 File Offset: 0x001D20F0
		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = GenPlant.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x001D3D3C File Offset: 0x001D213C
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, GenPlant.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(GenPlant.FallColorBegin, GenPlant.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x001D3D8C File Offset: 0x001D218C
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

		// Token: 0x060036C5 RID: 14021 RVA: 0x001D3E74 File Offset: 0x001D2274
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
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C8 RID: 1480
	public static class WorldGenerator
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x000F4C04 File Offset: 0x000F3004
		public static IEnumerable<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				return from x in DefDatabase<WorldGenStepDef>.AllDefs
				orderby x.order, x.index
				select x;
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000F4C64 File Offset: 0x000F3064
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature)
		{
			DeepProfiler.Start("GenerateWorld");
			Rand.PushState();
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.Seed = seedFromSeedString;
			World creatingWorld;
			try
			{
				Current.CreatingWorld = new World();
				Current.CreatingWorld.info.seedString = seedString;
				Current.CreatingWorld.info.planetCoverage = planetCoverage;
				Current.CreatingWorld.info.overallRainfall = overallRainfall;
				Current.CreatingWorld.info.overallTemperature = overallTemperature;
				Current.CreatingWorld.info.name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null);
				WorldGenerator.tmpGenSteps.Clear();
				WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
				for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("WorldGenStep - " + WorldGenerator.tmpGenSteps[i]);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
						WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFresh(seedString);
					}
					catch (Exception arg)
					{
						Log.Error("Error in WorldGenStep: " + arg, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
				Rand.Seed = seedFromSeedString;
				Current.CreatingWorld.grid.StandardizeTileData();
				Current.CreatingWorld.FinalizeInit();
				Find.Scenario.PostWorldGenerate();
				creatingWorld = Current.CreatingWorld;
			}
			finally
			{
				Rand.PopState();
				DeepProfiler.End();
				Current.CreatingWorld = null;
			}
			return creatingWorld;
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000F4E30 File Offset: 0x000F3230
		public static void GenerateWithoutWorldData(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateWithoutWorldData(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000F4EE4 File Offset: 0x000F32E4
		public static void GenerateFromScribe(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFromScribe(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000F4F98 File Offset: 0x000F3398
		private static int GetSeedPart(List<WorldGenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].worldGenStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (WorldGenerator.tmpGenSteps[i].worldGenStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000F4FF8 File Offset: 0x000F33F8
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x04001128 RID: 4392
		private static List<WorldGenStepDef> tmpGenSteps = new List<WorldGenStepDef>();

		// Token: 0x04001129 RID: 4393
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x0400112A RID: 4394
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x0400112B RID: 4395
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
	}
}

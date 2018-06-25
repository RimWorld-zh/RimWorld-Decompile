using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C6 RID: 1478
	public static class WorldGenerator
	{
		// Token: 0x04001125 RID: 4389
		private static List<WorldGenStepDef> tmpGenSteps = new List<WorldGenStepDef>();

		// Token: 0x04001126 RID: 4390
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x04001127 RID: 4391
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x04001128 RID: 4392
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x000F4E20 File Offset: 0x000F3220
		public static IEnumerable<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				return from x in DefDatabase<WorldGenStepDef>.AllDefs
				orderby x.order, x.index
				select x;
			}
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000F4E80 File Offset: 0x000F3280
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
				Current.CreatingWorld.info.name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null, null);
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

		// Token: 0x06001C7B RID: 7291 RVA: 0x000F504C File Offset: 0x000F344C
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

		// Token: 0x06001C7C RID: 7292 RVA: 0x000F5100 File Offset: 0x000F3500
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

		// Token: 0x06001C7D RID: 7293 RVA: 0x000F51B4 File Offset: 0x000F35B4
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

		// Token: 0x06001C7E RID: 7294 RVA: 0x000F5214 File Offset: 0x000F3614
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}
	}
}

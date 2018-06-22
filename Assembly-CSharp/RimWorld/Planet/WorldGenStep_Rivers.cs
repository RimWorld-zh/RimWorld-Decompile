using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BF RID: 1471
	public class WorldGenStep_Rivers : WorldGenStep
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x000F2F04 File Offset: 0x000F1304
		public override int SeedPart
		{
			get
			{
				return 605014749;
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000F2F1E File Offset: 0x000F131E
		public override void GenerateFresh(string seed)
		{
			this.GenerateRivers();
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000F2F28 File Offset: 0x000F1328
		private void GenerateRivers()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts();
			List<int> coastalWaterTiles = this.GetCoastalWaterTiles();
			if (coastalWaterTiles.Any<int>())
			{
				List<int> neighbors = new List<int>();
				List<int>[] array = Find.WorldPathFinder.FloodPathsWithCostForTree(coastalWaterTiles, delegate(int st, int ed)
				{
					Tile tile = Find.WorldGrid[ed];
					Tile tile2 = Find.WorldGrid[st];
					Find.WorldGrid.GetTileNeighbors(ed, neighbors);
					int num = neighbors[0];
					for (int j = 0; j < neighbors.Count; j++)
					{
						if (WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[neighbors[j]]) < WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[num]))
						{
							num = neighbors[j];
						}
					}
					float num2 = 1f;
					if (num != st)
					{
						num2 = 2f;
					}
					return Mathf.RoundToInt(num2 * WorldGenStep_Rivers.ElevationChangeCost.Evaluate(WorldGenStep_Rivers.GetImpliedElevation(tile2) - WorldGenStep_Rivers.GetImpliedElevation(tile)));
				}, (int tid) => Find.WorldGrid[tid].WaterCovered, null);
				float[] flow = new float[array.Length];
				for (int i = 0; i < coastalWaterTiles.Count; i++)
				{
					this.AccumulateFlow(flow, array, coastalWaterTiles[i]);
					this.CreateRivers(flow, array, coastalWaterTiles[i]);
				}
			}
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000F2FE4 File Offset: 0x000F13E4
		private static float GetImpliedElevation(Tile tile)
		{
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 250f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 500f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 1000f;
			}
			return tile.elevation + num;
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000F3060 File Offset: 0x000F1460
		private List<int> GetCoastalWaterTiles()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
			{
				Tile tile = Find.WorldGrid[i];
				if (tile.biome == BiomeDefOf.Ocean)
				{
					Find.WorldGrid.GetTileNeighbors(i, list2);
					bool flag = false;
					for (int j = 0; j < list2.Count; j++)
					{
						bool flag2 = Find.WorldGrid[list2[j]].biome != BiomeDefOf.Ocean;
						if (flag2)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(i);
					}
				}
			}
			return list;
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000F3130 File Offset: 0x000F1530
		private void AccumulateFlow(float[] flow, List<int>[] riverPaths, int index)
		{
			Tile tile = Find.WorldGrid[index];
			flow[index] += tile.rainfall;
			if (riverPaths[index] != null)
			{
				for (int i = 0; i < riverPaths[index].Count; i++)
				{
					this.AccumulateFlow(flow, riverPaths, riverPaths[index][i]);
					flow[index] += flow[riverPaths[index][i]];
				}
			}
			flow[index] = Mathf.Max(0f, flow[index] - WorldGenStep_Rivers.CalculateTotalEvaporation(flow[index], tile.temperature));
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000F31C8 File Offset: 0x000F15C8
		private void CreateRivers(float[] flow, List<int>[] riverPaths, int index)
		{
			List<int> list = new List<int>();
			Find.WorldGrid.GetTileNeighbors(index, list);
			for (int i = 0; i < list.Count; i++)
			{
				float targetFlow = flow[list[i]];
				RiverDef riverDef = (from rd in DefDatabase<RiverDef>.AllDefs
				where rd.spawnFlowThreshold > 0 && (float)rd.spawnFlowThreshold <= targetFlow
				select rd).MaxByWithFallback((RiverDef rd) => rd.spawnFlowThreshold, null);
				if (riverDef != null && Rand.Value < riverDef.spawnChance)
				{
					Find.WorldGrid.OverlayRiver(index, list[i], riverDef);
					this.ExtendRiver(flow, riverPaths, list[i], riverDef);
				}
			}
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000F328C File Offset: 0x000F168C
		private void ExtendRiver(float[] flow, List<int>[] riverPaths, int index, RiverDef incomingRiver)
		{
			if (riverPaths[index] != null)
			{
				int bestOutput = riverPaths[index].MaxBy((int ni) => flow[ni]);
				RiverDef riverDef = incomingRiver;
				while (riverDef != null && (float)riverDef.degradeThreshold > flow[bestOutput])
				{
					riverDef = riverDef.degradeChild;
				}
				if (riverDef != null)
				{
					Find.WorldGrid.OverlayRiver(index, bestOutput, riverDef);
					this.ExtendRiver(flow, riverPaths, bestOutput, riverDef);
				}
				if (incomingRiver.branches != null)
				{
					using (IEnumerator<int> enumerator = (from ni in riverPaths[index]
					where ni != bestOutput
					select ni).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int alternateRiver = enumerator.Current;
							RiverDef.Branch branch2 = (from branch in incomingRiver.branches
							where (float)branch.minFlow <= flow[alternateRiver]
							select branch).MaxByWithFallback((RiverDef.Branch branch) => branch.minFlow, null);
							if (branch2 != null && Rand.Value < branch2.chance)
							{
								Find.WorldGrid.OverlayRiver(index, alternateRiver, branch2.child);
								this.ExtendRiver(flow, riverPaths, alternateRiver, branch2.child);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000F342C File Offset: 0x000F182C
		public static float CalculateEvaporationConstant(float temperature)
		{
			float num = 0.61121f * Mathf.Exp((18.678f - temperature / 234.5f) * (temperature / (257.14f + temperature)));
			return num / (temperature + 273f);
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000F3470 File Offset: 0x000F1870
		public static float CalculateRiverSurfaceArea(float flow)
		{
			return Mathf.Pow(flow, 0.5f);
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000F3490 File Offset: 0x000F1890
		public static float CalculateEvaporativeArea(float flow)
		{
			return WorldGenStep_Rivers.CalculateRiverSurfaceArea(flow);
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000F34AC File Offset: 0x000F18AC
		public static float CalculateTotalEvaporation(float flow, float temperature)
		{
			return WorldGenStep_Rivers.CalculateEvaporationConstant(temperature) * WorldGenStep_Rivers.CalculateEvaporativeArea(flow) * 250f;
		}

		// Token: 0x040010E9 RID: 4329
		private static readonly SimpleCurve ElevationChangeCost = new SimpleCurve
		{
			{
				new CurvePoint(-1000f, 50f),
				true
			},
			{
				new CurvePoint(-100f, 100f),
				true
			},
			{
				new CurvePoint(0f, 400f),
				true
			},
			{
				new CurvePoint(0f, 5000f),
				true
			},
			{
				new CurvePoint(100f, 50000f),
				true
			},
			{
				new CurvePoint(1000f, 50000f),
				true
			}
		};

		// Token: 0x040010EA RID: 4330
		private const float HillinessSmallHillsElevation = 15f;

		// Token: 0x040010EB RID: 4331
		private const float HillinessLargeHillsElevation = 250f;

		// Token: 0x040010EC RID: 4332
		private const float HillinessMountainousElevation = 500f;

		// Token: 0x040010ED RID: 4333
		private const float HillinessImpassableElevation = 1000f;

		// Token: 0x040010EE RID: 4334
		private const float NonRiverEvaporation = 0f;

		// Token: 0x040010EF RID: 4335
		private const float EvaporationMultiple = 250f;
	}
}

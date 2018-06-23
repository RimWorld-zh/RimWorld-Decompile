using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000295 RID: 661
	public abstract class FeatureWorker
	{
		// Token: 0x040005EB RID: 1515
		public FeatureDef def;

		// Token: 0x040005EC RID: 1516
		protected static bool[] visited;

		// Token: 0x040005ED RID: 1517
		protected static int[] groupSize;

		// Token: 0x040005EE RID: 1518
		protected static int[] groupID;

		// Token: 0x040005EF RID: 1519
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x040005F0 RID: 1520
		private static HashSet<int> tmpTilesForTextDrawPosCalculationSet = new HashSet<int>();

		// Token: 0x040005F1 RID: 1521
		private static List<int> tmpEdgeTiles = new List<int>();

		// Token: 0x040005F2 RID: 1522
		private static List<Pair<int, int>> tmpTraversedTiles = new List<Pair<int, int>>();

		// Token: 0x06000B2A RID: 2858
		public abstract void GenerateWhereAppropriate();

		// Token: 0x06000B2B RID: 2859 RVA: 0x00065790 File Offset: 0x00063B90
		protected void AddFeature(List<int> members, List<int> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature();
			worldFeature.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
			worldFeature.def = this.def;
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "r_name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00065854 File Offset: 0x00063C54
		private void AssignBestDrawPos(WorldFeature newFeature, List<int> tilesForTextDrawPosCalculation)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				int num = tilesForTextDrawPosCalculation[i];
				vector += worldGrid.GetTileCenter(num);
				bool flag = worldGrid.IsOnEdge(num);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(num, FeatureWorker.tmpNeighbors);
					for (int j = 0; j < FeatureWorker.tmpNeighbors.Count; j++)
					{
						if (!FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[j]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(num);
				}
			}
			vector /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any<int>())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement<int>());
			}
			int bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> passCheck = (int x) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(x);
			Func<int, int, bool> processor = delegate(int tile, int traversalDist)
			{
				FeatureWorker.tmpTraversedTiles.Add(new Pair<int, int>(tile, traversalDist));
				bestTileDist = traversalDist;
				return false;
			};
			List<int> extraRootTiles = FeatureWorker.tmpEdgeTiles;
			worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, extraRootTiles);
			int num2 = -1;
			float num3 = -1f;
			for (int k = 0; k < FeatureWorker.tmpTraversedTiles.Count; k++)
			{
				if (FeatureWorker.tmpTraversedTiles[k].Second == bestTileDist)
				{
					Vector3 tileCenter = worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[k].First);
					float sqrMagnitude = (tileCenter - vector).sqrMagnitude;
					if (num2 == -1 || sqrMagnitude < num3)
					{
						num2 = FeatureWorker.tmpTraversedTiles[k].First;
						num3 = sqrMagnitude;
					}
				}
			}
			float maxDrawSizeInTiles = (float)bestTileDist * 2f * 1.2f;
			newFeature.drawCenter = worldGrid.GetTileCenter(num2);
			newFeature.maxDrawSizeInTiles = maxDrawSizeInTiles;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00065AA1 File Offset: 0x00063EA1
		protected static void ClearVisited()
		{
			FeatureWorker.ClearOrCreate<bool>(ref FeatureWorker.visited);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00065AAE File Offset: 0x00063EAE
		protected static void ClearGroupSizes()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupSize);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00065ABB File Offset: 0x00063EBB
		protected static void ClearGroupIDs()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupID);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00065AC8 File Offset: 0x00063EC8
		private static void ClearOrCreate<T>(ref T[] array)
		{
			int tilesCount = Find.WorldGrid.TilesCount;
			if (array == null || array.Length != tilesCount)
			{
				array = new T[tilesCount];
			}
			else
			{
				Array.Clear(array, 0, array.Length);
			}
		}
	}
}

using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class FeatureWorker
	{
		public FeatureDef def;

		protected static bool[] visited;

		protected static int[] groupSize;

		protected static int[] groupID;

		private static List<int> tmpNeighbors = new List<int>();

		private static HashSet<int> tmpTilesForTextDrawPosCalculationSet = new HashSet<int>();

		private static List<int> tmpEdgeTiles = new List<int>();

		private static List<Pair<int, int>> tmpTraversedTiles = new List<Pair<int, int>>();

		public abstract void GenerateWhereAppropriate();

		protected void AddFeature(List<int> members, List<int> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature();
			worldFeature.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
			worldFeature.def = this.def;
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		private void AssignBestDrawPos(WorldFeature newFeature, List<int> tilesForTextDrawPosCalculation)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 a = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				int num = tilesForTextDrawPosCalculation[i];
				a += worldGrid.GetTileCenter(num);
				bool flag = worldGrid.IsOnEdge(num);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(num, FeatureWorker.tmpNeighbors);
					int num2 = 0;
					while (num2 < FeatureWorker.tmpNeighbors.Count)
					{
						if (FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[num2]))
						{
							num2++;
							continue;
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(num);
				}
			}
			a /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement());
			}
			int bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> passCheck = (Predicate<int>)((int x) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(x));
			Func<int, int, bool> processor = (Func<int, int, bool>)delegate(int tile, int traversalDist)
			{
				FeatureWorker.tmpTraversedTiles.Add(new Pair<int, int>(tile, traversalDist));
				bestTileDist = traversalDist;
				return false;
			};
			List<int> extraRootTiles = FeatureWorker.tmpEdgeTiles;
			worldFloodFiller.FloodFill(rootTile, passCheck, processor, 2147483647, extraRootTiles);
			int num3 = -1;
			float num4 = -1f;
			for (int j = 0; j < FeatureWorker.tmpTraversedTiles.Count; j++)
			{
				if (FeatureWorker.tmpTraversedTiles[j].Second == bestTileDist)
				{
					Vector3 tileCenter = worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[j].First);
					float sqrMagnitude = (tileCenter - a).sqrMagnitude;
					if (num3 == -1 || sqrMagnitude < num4)
					{
						num3 = FeatureWorker.tmpTraversedTiles[j].First;
						num4 = sqrMagnitude;
					}
				}
			}
			float num5 = (float)((float)bestTileDist * 2.0 * 1.2000000476837158);
			newFeature.drawCenter = worldGrid.GetTileCenter(num3);
			newFeature.maxDrawSizeInTiles = new Vector2(num5, num5);
		}

		protected static void ClearVisited()
		{
			FeatureWorker.ClearOrCreate<bool>(ref FeatureWorker.visited);
		}

		protected static void ClearGroupSizes()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupSize);
		}

		protected static void ClearGroupIDs()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupID);
		}

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

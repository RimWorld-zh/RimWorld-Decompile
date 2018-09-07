using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
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

		[CompilerGenerated]
		private static Func<WorldFeature, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache1;

		protected FeatureWorker()
		{
		}

		public abstract void GenerateWhereAppropriate();

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

		// Note: this type is marked as 'beforefieldinit'.
		static FeatureWorker()
		{
		}

		[CompilerGenerated]
		private static string <AddFeature>m__0(WorldFeature x)
		{
			return x.name;
		}

		[CompilerGenerated]
		private static bool <AssignBestDrawPos>m__1(int x)
		{
			return FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(x);
		}

		[CompilerGenerated]
		private sealed class <AssignBestDrawPos>c__AnonStorey0
		{
			internal int bestTileDist;

			public <AssignBestDrawPos>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int tile, int traversalDist)
			{
				FeatureWorker.tmpTraversedTiles.Add(new Pair<int, int>(tile, traversalDist));
				this.bestTileDist = traversalDist;
				return false;
			}
		}
	}
}

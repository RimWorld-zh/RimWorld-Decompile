using System;
using System.Collections.Generic;

namespace Verse
{
	public static class FloodFillerFog
	{
		private static bool testMode = false;

		private static List<IntVec3> cellsToUnfog = new List<IntVec3>(1024);

		private const int MaxNumTestUnfog = 500;

		public static FloodUnfogResult FloodUnfog(IntVec3 root, Map map)
		{
			FloodFillerFog.cellsToUnfog.Clear();
			FloodUnfogResult result = default(FloodUnfogResult);
			bool[] fogGridDirect = map.fogGrid.fogGrid;
			FogGrid fogGrid = map.fogGrid;
			List<IntVec3> newlyUnfoggedCells = new List<IntVec3>();
			int numUnfogged = 0;
			bool expanding = false;
			Predicate<IntVec3> predicate = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				bool result2;
				if (!fogGridDirect[map.cellIndices.CellToIndex(c)])
				{
					result2 = false;
				}
				else
				{
					Thing edifice = c.GetEdifice(map);
					result2 = ((byte)((edifice == null || !edifice.def.MakeFog) ? ((!FloodFillerFog.testMode || expanding || numUnfogged <= 500) ? 1 : 0) : 0) != 0);
				}
				return result2;
			};
			Action<IntVec3> processor = (Action<IntVec3>)delegate(IntVec3 c)
			{
				fogGrid.Unfog(c);
				newlyUnfoggedCells.Add(c);
				List<Thing> thingList = c.GetThingList(map);
				for (int l = 0; l < thingList.Count; l++)
				{
					Pawn pawn = thingList[l] as Pawn;
					if (pawn != null)
					{
						pawn.mindState.Active = true;
						if (pawn.def.race.IsMechanoid)
						{
							result.mechanoidFound = true;
						}
					}
				}
				if (FloodFillerFog.testMode)
				{
					numUnfogged++;
					map.debugDrawer.FlashCell(c, (float)((float)numUnfogged / 200.0), numUnfogged.ToStringCached(), 50);
				}
			};
			map.floodFiller.FloodFill(root, predicate, processor, 2147483647, false, null);
			expanding = true;
			for (int i = 0; i < newlyUnfoggedCells.Count; i++)
			{
				IntVec3 a = newlyUnfoggedCells[i];
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec = a + GenAdj.AdjacentCells[j];
					if (intVec.InBounds(map) && fogGrid.IsFogged(intVec) && !predicate(intVec))
					{
						FloodFillerFog.cellsToUnfog.Add(intVec);
					}
				}
			}
			for (int k = 0; k < FloodFillerFog.cellsToUnfog.Count; k++)
			{
				fogGrid.Unfog(FloodFillerFog.cellsToUnfog[k]);
				if (FloodFillerFog.testMode)
				{
					map.debugDrawer.FlashCell(FloodFillerFog.cellsToUnfog[k], 0.3f, "x", 50);
				}
			}
			FloodFillerFog.cellsToUnfog.Clear();
			return result;
		}

		internal static void DebugFloodUnfog(IntVec3 root, Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 allCell in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(allCell, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.testMode = true;
			FloodFillerFog.FloodUnfog(root, map);
			FloodFillerFog.testMode = false;
		}

		internal static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 allCell in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(allCell, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement().Position, map);
		}
	}
}

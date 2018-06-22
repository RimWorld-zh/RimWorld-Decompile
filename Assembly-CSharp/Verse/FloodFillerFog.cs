using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C0E RID: 3086
	public static class FloodFillerFog
	{
		// Token: 0x06004384 RID: 17284 RVA: 0x0023AD64 File Offset: 0x00239164
		public static FloodUnfogResult FloodUnfog(IntVec3 root, Map map)
		{
			FloodFillerFog.cellsToUnfog.Clear();
			FloodUnfogResult result = default(FloodUnfogResult);
			bool[] fogGridDirect = map.fogGrid.fogGrid;
			FogGrid fogGrid = map.fogGrid;
			List<IntVec3> newlyUnfoggedCells = new List<IntVec3>();
			int numUnfogged = 0;
			bool expanding = false;
			CellRect viewRect = CellRect.ViewRect(map);
			result.allOnScreen = true;
			Predicate<IntVec3> predicate = delegate(IntVec3 c)
			{
				bool result;
				if (!fogGridDirect[map.cellIndices.CellToIndex(c)])
				{
					result = false;
				}
				else
				{
					Thing edifice = c.GetEdifice(map);
					result = ((edifice == null || !edifice.def.MakeFog) && (!FloodFillerFog.testMode || expanding || numUnfogged <= 500));
				}
				return result;
			};
			Action<IntVec3> processor = delegate(IntVec3 c)
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
				if (!viewRect.Contains(c))
				{
					result.allOnScreen = false;
				}
				result.cellsUnfogged++;
				if (FloodFillerFog.testMode)
				{
					numUnfogged++;
					map.debugDrawer.FlashCell(c, (float)numUnfogged / 200f, numUnfogged.ToStringCached(), 50);
				}
			};
			map.floodFiller.FloodFill(root, predicate, processor, int.MaxValue, false, null);
			expanding = true;
			for (int i = 0; i < newlyUnfoggedCells.Count; i++)
			{
				IntVec3 a = newlyUnfoggedCells[i];
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec = a + GenAdj.AdjacentCells[j];
					if (intVec.InBounds(map) && fogGrid.IsFogged(intVec))
					{
						if (!predicate(intVec))
						{
							FloodFillerFog.cellsToUnfog.Add(intVec);
						}
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

		// Token: 0x06004385 RID: 17285 RVA: 0x0023AF60 File Offset: 0x00239360
		internal static void DebugFloodUnfog(IntVec3 root, Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.testMode = true;
			FloodFillerFog.FloodUnfog(root, map);
			FloodFillerFog.testMode = false;
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0023AFE4 File Offset: 0x002393E4
		internal static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>().Position, map);
		}

		// Token: 0x04002E23 RID: 11811
		private static bool testMode = false;

		// Token: 0x04002E24 RID: 11812
		private static List<IntVec3> cellsToUnfog = new List<IntVec3>(1024);

		// Token: 0x04002E25 RID: 11813
		private const int MaxNumTestUnfog = 500;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

		internal static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>().Position, map);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FloodFillerFog()
		{
		}

		[CompilerGenerated]
		private sealed class <FloodUnfog>c__AnonStorey0
		{
			internal bool[] fogGridDirect;

			internal Map map;

			internal bool expanding;

			internal int numUnfogged;

			internal FogGrid fogGrid;

			internal List<IntVec3> newlyUnfoggedCells;

			internal FloodUnfogResult result;

			internal CellRect viewRect;

			public <FloodUnfog>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool flag;
				if (!this.fogGridDirect[this.map.cellIndices.CellToIndex(c)])
				{
					flag = false;
				}
				else
				{
					Thing edifice = c.GetEdifice(this.map);
					flag = ((edifice == null || !edifice.def.MakeFog) && (!FloodFillerFog.testMode || this.expanding || this.numUnfogged <= 500));
				}
				return flag;
			}

			internal void <>m__1(IntVec3 c)
			{
				this.fogGrid.Unfog(c);
				this.newlyUnfoggedCells.Add(c);
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null)
					{
						pawn.mindState.Active = true;
						if (pawn.def.race.IsMechanoid)
						{
							this.result.mechanoidFound = true;
						}
					}
				}
				if (!this.viewRect.Contains(c))
				{
					this.result.allOnScreen = false;
				}
				this.result.cellsUnfogged = this.result.cellsUnfogged + 1;
				if (FloodFillerFog.testMode)
				{
					this.numUnfogged++;
					this.map.debugDrawer.FlashCell(c, (float)this.numUnfogged / 200f, this.numUnfogged.ToStringCached(), 50);
				}
			}
		}
	}
}

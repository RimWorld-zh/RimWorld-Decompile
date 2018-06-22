using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A91 RID: 2705
	[HasDebugOutput]
	public sealed class PathGrid
	{
		// Token: 0x06003C17 RID: 15383 RVA: 0x001FC54C File Offset: 0x001FA94C
		public PathGrid(Map map)
		{
			this.map = map;
			this.ResetPathGrid();
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x001FC562 File Offset: 0x001FA962
		public void ResetPathGrid()
		{
			this.pathGrid = new int[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x001FC580 File Offset: 0x001FA980
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x001FC5CC File Offset: 0x001FA9CC
		public bool WalkableFast(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)] < 10000;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x001FC600 File Offset: 0x001FAA00
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x001FC638 File Offset: 0x001FAA38
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x001FC65C File Offset: 0x001FAA5C
		public int PerceivedPathCostAt(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x001FC68C File Offset: 0x001FAA8C
		public void RecalculatePerceivedPathCostUnderThing(Thing t)
		{
			if (t.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(t.Position);
			}
			else
			{
				CellRect cellRect = t.OccupiedRect();
				for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
				{
					for (int j = cellRect.minX; j <= cellRect.maxX; j++)
					{
						IntVec3 c = new IntVec3(j, 0, i);
						this.RecalculatePerceivedPathCostAt(c);
					}
				}
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x001FC720 File Offset: 0x001FAB20
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				bool flag = this.WalkableFast(c);
				this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid);
				if (this.WalkableFast(c) != flag)
				{
					this.map.reachability.ClearCache();
					this.map.regionDirtyer.Notify_WalkabilityChanged(c);
				}
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x001FC7A4 File Offset: 0x001FABA4
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x001FC808 File Offset: 0x001FAC08
		public int CalculatedCostAt(IntVec3 c, bool perceivedStatic, IntVec3 prevCell)
		{
			int num = 0;
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
			if (terrainDef == null || terrainDef.passability == Traversability.Impassable)
			{
				num = 10000;
			}
			else
			{
				num += terrainDef.pathCost;
			}
			int num2 = SnowUtility.MovementTicksAddOn(this.map.snowGrid.GetCategory(c));
			num += num2;
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			int num3 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.passability == Traversability.Impassable)
				{
					return 10000;
				}
				if (num3 < 450 && (!PathGrid.IsPathCostIgnoreRepeater(thing.def) || !prevCell.IsValid || !this.ContainsPathCostIgnoreRepeater(prevCell)))
				{
					num += Mathf.Min(thing.def.pathCost, 450 - num3);
					num3 += thing.def.pathCost;
				}
				if (prevCell.IsValid && thing is Building_Door)
				{
					Building edifice = prevCell.GetEdifice(this.map);
					if (edifice != null && edifice is Building_Door)
					{
						num += 45;
					}
				}
			}
			if (perceivedStatic)
			{
				for (int j = 0; j < 9; j++)
				{
					IntVec3 b = GenAdj.AdjacentCellsAndInside[j];
					IntVec3 c2 = c + b;
					if (c2.InBounds(this.map))
					{
						Fire fire = null;
						list = this.map.thingGrid.ThingsListAtFast(c2);
						for (int k = 0; k < list.Count; k++)
						{
							fire = (list[k] as Fire);
							if (fire != null)
							{
								break;
							}
						}
						if (fire != null && fire.parent == null)
						{
							if (b.x == 0 && b.z == 0)
							{
								num += 1000;
							}
							else
							{
								num += 150;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x001FCA5C File Offset: 0x001FAE5C
		private bool ContainsPathCostIgnoreRepeater(IntVec3 c)
		{
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(list[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x001FCABC File Offset: 0x001FAEBC
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x001FCAE8 File Offset: 0x001FAEE8
		[DebugOutput]
		public static void ThingPathCostsIgnoreRepeaters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===============PATH COST IGNORE REPEATERS==============");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(thingDef) && thingDef.passability != Traversability.Impassable)
				{
					stringBuilder.AppendLine(thingDef.defName + " " + thingDef.pathCost);
				}
			}
			stringBuilder.AppendLine("===============NON-PATH COST IGNORE REPEATERS that are buildings with >0 pathCost ==============");
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (!PathGrid.IsPathCostIgnoreRepeater(thingDef2) && thingDef2.passability != Traversability.Impassable && thingDef2.category == ThingCategory.Building && thingDef2.pathCost > 0)
				{
					stringBuilder.AppendLine(thingDef2.defName + " " + thingDef2.pathCost);
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040025F0 RID: 9712
		private Map map;

		// Token: 0x040025F1 RID: 9713
		public int[] pathGrid;

		// Token: 0x040025F2 RID: 9714
		public const int ImpassableCost = 10000;

		// Token: 0x040025F3 RID: 9715
		private const int MaxThingsPathCost = 450;
	}
}

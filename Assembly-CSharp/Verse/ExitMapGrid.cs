using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C23 RID: 3107
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x060043F0 RID: 17392 RVA: 0x0023C540 File Offset: 0x0023A940
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x0023C558 File Offset: 0x0023A958
		public bool MapUsesExitGrid
		{
			get
			{
				bool result;
				if (this.map.IsPlayerHome)
				{
					result = false;
				}
				else
				{
					CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
					if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
					{
						result = false;
					}
					else
					{
						FormCaravanComp component = this.map.Parent.GetComponent<FormCaravanComp>();
						result = (component == null || !component.CanFormOrReformCaravanNow);
					}
				}
				return result;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x0023C5E8 File Offset: 0x0023A9E8
		public CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if (!this.MapUsesExitGrid)
				{
					result = null;
				}
				else
				{
					if (this.dirty)
					{
						this.Rebuild();
					}
					if (this.drawerInt == null)
					{
						this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 0.33f);
					}
					result = this.drawerInt;
				}
				return result;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x0023C668 File Offset: 0x0023AA68
		public BoolGrid Grid
		{
			get
			{
				BoolGrid result;
				if (!this.MapUsesExitGrid)
				{
					result = null;
				}
				else
				{
					if (this.dirty)
					{
						this.Rebuild();
					}
					result = this.exitMapGrid;
				}
				return result;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060043F4 RID: 17396 RVA: 0x0023C6A8 File Offset: 0x0023AAA8
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0023C6D8 File Offset: 0x0023AAD8
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0023C718 File Offset: 0x0023AB18
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0023C734 File Offset: 0x0023AB34
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0023C767 File Offset: 0x0023AB67
		public void ExitMapGridUpdate()
		{
			if (this.MapUsesExitGrid)
			{
				this.Drawer.MarkForDraw();
				this.Drawer.CellBoolDrawerUpdate();
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0023C790 File Offset: 0x0023AB90
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0023C79A File Offset: 0x0023AB9A
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0023C7A4 File Offset: 0x0023ABA4
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0023C8A8 File Offset: 0x0023ACA8
		private bool IsGoodExitCell(IntVec3 cell)
		{
			bool result;
			if (!cell.CanBeSeenOver(this.map))
			{
				result = false;
			}
			else
			{
				int num = GenRadial.NumCellsInRadius(2f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = cell + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map, false, null, 0, 0))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04002E55 RID: 11861
		private Map map;

		// Token: 0x04002E56 RID: 11862
		private bool dirty = true;

		// Token: 0x04002E57 RID: 11863
		private BoolGrid exitMapGrid;

		// Token: 0x04002E58 RID: 11864
		private CellBoolDrawer drawerInt;

		// Token: 0x04002E59 RID: 11865
		private const int MaxDistToEdge = 2;
	}
}

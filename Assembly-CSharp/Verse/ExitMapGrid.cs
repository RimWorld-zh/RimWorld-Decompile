using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C24 RID: 3108
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x060043F2 RID: 17394 RVA: 0x0023C568 File Offset: 0x0023A968
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x0023C580 File Offset: 0x0023A980
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

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060043F4 RID: 17396 RVA: 0x0023C610 File Offset: 0x0023AA10
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

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060043F5 RID: 17397 RVA: 0x0023C690 File Offset: 0x0023AA90
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

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0023C6D0 File Offset: 0x0023AAD0
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0023C700 File Offset: 0x0023AB00
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0023C740 File Offset: 0x0023AB40
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0023C75C File Offset: 0x0023AB5C
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0023C78F File Offset: 0x0023AB8F
		public void ExitMapGridUpdate()
		{
			if (this.MapUsesExitGrid)
			{
				this.Drawer.MarkForDraw();
				this.Drawer.CellBoolDrawerUpdate();
			}
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0023C7B8 File Offset: 0x0023ABB8
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0023C7C2 File Offset: 0x0023ABC2
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0023C7CC File Offset: 0x0023ABCC
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

		// Token: 0x060043FE RID: 17406 RVA: 0x0023C8D0 File Offset: 0x0023ACD0
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

		// Token: 0x04002E57 RID: 11863
		private Map map;

		// Token: 0x04002E58 RID: 11864
		private bool dirty = true;

		// Token: 0x04002E59 RID: 11865
		private BoolGrid exitMapGrid;

		// Token: 0x04002E5A RID: 11866
		private CellBoolDrawer drawerInt;

		// Token: 0x04002E5B RID: 11867
		private const int MaxDistToEdge = 2;
	}
}

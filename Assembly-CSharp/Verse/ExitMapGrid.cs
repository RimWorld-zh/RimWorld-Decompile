using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C23 RID: 3107
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x04002E66 RID: 11878
		private Map map;

		// Token: 0x04002E67 RID: 11879
		private bool dirty = true;

		// Token: 0x04002E68 RID: 11880
		private BoolGrid exitMapGrid;

		// Token: 0x04002E69 RID: 11881
		private CellBoolDrawer drawerInt;

		// Token: 0x04002E6A RID: 11882
		private const int MaxDistToEdge = 2;

		// Token: 0x060043FC RID: 17404 RVA: 0x0023DCC4 File Offset: 0x0023C0C4
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060043FD RID: 17405 RVA: 0x0023DCDC File Offset: 0x0023C0DC
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
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x0023DD6C File Offset: 0x0023C16C
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
		// (get) Token: 0x060043FF RID: 17407 RVA: 0x0023DDEC File Offset: 0x0023C1EC
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
		// (get) Token: 0x06004400 RID: 17408 RVA: 0x0023DE2C File Offset: 0x0023C22C
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0023DE5C File Offset: 0x0023C25C
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0023DE9C File Offset: 0x0023C29C
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0023DEB8 File Offset: 0x0023C2B8
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0023DEEB File Offset: 0x0023C2EB
		public void ExitMapGridUpdate()
		{
			if (this.MapUsesExitGrid)
			{
				this.Drawer.MarkForDraw();
				this.Drawer.CellBoolDrawerUpdate();
			}
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0023DF14 File Offset: 0x0023C314
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0023DF1E File Offset: 0x0023C31E
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0023DF28 File Offset: 0x0023C328
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

		// Token: 0x06004408 RID: 17416 RVA: 0x0023E02C File Offset: 0x0023C42C
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
	}
}

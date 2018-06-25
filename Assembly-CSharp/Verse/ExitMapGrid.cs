using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		private Map map;

		private bool dirty = true;

		private BoolGrid exitMapGrid;

		private CellBoolDrawer drawerInt;

		private const int MaxDistToEdge = 2;

		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

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

		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		public void ExitMapGridUpdate()
		{
			if (this.MapUsesExitGrid)
			{
				this.Drawer.MarkForDraw();
				this.Drawer.CellBoolDrawerUpdate();
			}
		}

		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

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

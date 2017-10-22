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
					CaravansBattlefield caravansBattlefield = this.map.info.parent as CaravansBattlefield;
					result = ((byte)((caravansBattlefield == null || !caravansBattlefield.def.blockExitGridUntilBattleIsWon || caravansBattlefield.WonBattle) ? 1 : 0) != 0);
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
						IntVec3 size = this.map.Size;
						int x = size.x;
						IntVec3 size2 = this.map.Size;
						this.drawerInt = new CellBoolDrawer(this, x, size2.z, 0.33f);
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

		public ExitMapGrid(Map map)
		{
			this.map = map;
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
				for (int num2 = 0; num2 < num; num2++)
				{
					IntVec3 intVec = cell + GenRadial.RadialPattern[num2];
					if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map, false, null, 0, 0))
						goto IL_008c;
				}
				result = false;
			}
			goto IL_00a6;
			IL_00a6:
			return result;
			IL_008c:
			result = true;
			goto IL_00a6;
		}
	}
}

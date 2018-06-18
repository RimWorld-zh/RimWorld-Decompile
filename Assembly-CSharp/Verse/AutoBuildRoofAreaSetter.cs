using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C9C RID: 3228
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x0600470A RID: 18186 RVA: 0x00256AE6 File Offset: 0x00254EE6
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00256B24 File Offset: 0x00254F24
		public void TryGenerateAreaOnImpassable(IntVec3 c)
		{
			if (!c.Roofed(this.map) && c.Impassable(this.map) && RoofCollapseUtility.WithinRangeOfRoofHolder(c, this.map, false))
			{
				bool flag = false;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 loc = c + GenRadial.RadialPattern[i];
					Room room = loc.GetRoom(this.map, RegionType.Set_Passable);
					if (room != null && !room.TouchesMapEdge)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.map.areaManager.BuildRoof[c] = true;
					MoteMaker.PlaceTempRoof(c, this.map);
				}
			}
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00256BE8 File Offset: 0x00254FE8
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00256BF7 File Offset: 0x00254FF7
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00256C00 File Offset: 0x00255000
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00256C4C File Offset: 0x0025504C
		private void TryGenerateAreaNow(Room room)
		{
			if (!room.Dereferenced && !room.TouchesMapEdge)
			{
				if (room.RegionCount <= 26 && room.CellCount <= 320)
				{
					if (room.RegionType != RegionType.Portal)
					{
						bool flag = false;
						foreach (IntVec3 c in room.BorderCells)
						{
							Thing roofHolderOrImpassable = c.GetRoofHolderOrImpassable(this.map);
							if (roofHolderOrImpassable != null)
							{
								if (roofHolderOrImpassable.Faction != null && roofHolderOrImpassable.Faction != Faction.OfPlayer)
								{
									return;
								}
								if (roofHolderOrImpassable.def.building != null && !roofHolderOrImpassable.def.building.allowAutoroof)
								{
									return;
								}
								if (roofHolderOrImpassable.Faction == Faction.OfPlayer)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							this.innerCells.Clear();
							foreach (IntVec3 intVec in room.Cells)
							{
								if (!this.innerCells.Contains(intVec))
								{
									this.innerCells.Add(intVec);
								}
								for (int i = 0; i < 8; i++)
								{
									IntVec3 c2 = intVec + GenAdj.AdjacentCells[i];
									if (c2.InBounds(this.map))
									{
										Thing roofHolderOrImpassable2 = c2.GetRoofHolderOrImpassable(this.map);
										if (roofHolderOrImpassable2 != null && (roofHolderOrImpassable2.def.size.x > 1 || roofHolderOrImpassable2.def.size.z > 1))
										{
											CellRect cellRect = roofHolderOrImpassable2.OccupiedRect();
											cellRect.ClipInsideMap(this.map);
											for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
											{
												for (int k = cellRect.minX; k <= cellRect.maxX; k++)
												{
													IntVec3 item = new IntVec3(k, 0, j);
													if (!this.innerCells.Contains(item))
													{
														this.innerCells.Add(item);
													}
												}
											}
										}
									}
								}
							}
							this.cellsToRoof.Clear();
							foreach (IntVec3 a in this.innerCells)
							{
								for (int l = 0; l < 9; l++)
								{
									IntVec3 intVec2 = a + GenAdj.AdjacentCellsAndInside[l];
									if (intVec2.InBounds(this.map) && (l == 8 || intVec2.GetRoofHolderOrImpassable(this.map) != null) && !this.cellsToRoof.Contains(intVec2))
									{
										this.cellsToRoof.Add(intVec2);
									}
								}
							}
							this.justRoofedCells.Clear();
							foreach (IntVec3 intVec3 in this.cellsToRoof)
							{
								if (this.map.roofGrid.RoofAt(intVec3) == null && !this.justRoofedCells.Contains(intVec3))
								{
									if (!this.map.areaManager.NoRoof[intVec3] && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec3, this.map, true))
									{
										this.map.areaManager.BuildRoof[intVec3] = true;
										this.justRoofedCells.Add(intVec3);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04003048 RID: 12360
		private Map map;

		// Token: 0x04003049 RID: 12361
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x0400304A RID: 12362
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x0400304B RID: 12363
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x0400304C RID: 12364
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}

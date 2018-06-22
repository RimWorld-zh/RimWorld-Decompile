using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C99 RID: 3225
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x06004713 RID: 18195 RVA: 0x00257ED6 File Offset: 0x002562D6
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00257F14 File Offset: 0x00256314
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

		// Token: 0x06004715 RID: 18197 RVA: 0x00257FD8 File Offset: 0x002563D8
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00257FE7 File Offset: 0x002563E7
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00257FF0 File Offset: 0x002563F0
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0025803C File Offset: 0x0025643C
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

		// Token: 0x04003053 RID: 12371
		private Map map;

		// Token: 0x04003054 RID: 12372
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x04003055 RID: 12373
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x04003056 RID: 12374
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x04003057 RID: 12375
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}

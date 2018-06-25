using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C12 RID: 3090
	public sealed class FogGrid : IExposable
	{
		// Token: 0x04002E2D RID: 11821
		private Map map;

		// Token: 0x04002E2E RID: 11822
		public bool[] fogGrid;

		// Token: 0x04002E2F RID: 11823
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;

		// Token: 0x0600438B RID: 17291 RVA: 0x0023B5DF File Offset: 0x002399DF
		public FogGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0023B5EF File Offset: 0x002399EF
		public void ExposeData()
		{
			DataExposeUtility.BoolArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0023B610 File Offset: 0x00239A10
		public void Unfog(IntVec3 c)
		{
			this.UnfogWorker(c);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.Fillage == FillCategory.Full)
				{
					foreach (IntVec3 c2 in thing.OccupiedRect().Cells)
					{
						this.UnfogWorker(c2);
					}
				}
			}
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0023B6C4 File Offset: 0x00239AC4
		private void UnfogWorker(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.fogGrid[num])
			{
				this.fogGrid[num] = false;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.FogOfWar);
				}
				Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.Mine);
				if (designation != null && c.GetFirstMineable(this.map) == null)
				{
					designation.Delete();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.map.roofGrid.Drawer.SetDirty();
				}
			}
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0023B770 File Offset: 0x00239B70
		public bool IsFogged(IntVec3 c)
		{
			return c.InBounds(this.map) && this.fogGrid != null && this.fogGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0023B7C0 File Offset: 0x00239BC0
		public bool IsFogged(int index)
		{
			return this.fogGrid[index];
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0023B7E0 File Offset: 0x00239BE0
		public void ClearAllFog()
		{
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.Unfog(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0023B848 File Offset: 0x00239C48
		public void Notify_FogBlockerRemoved(IntVec3 c)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					if (c2.InBounds(this.map) && !this.IsFogged(c2))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.FloodUnfogAdjacent(c);
				}
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0023B8CA File Offset: 0x00239CCA
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer)
			{
				this.FloodUnfogAdjacent(door.Position);
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0023B900 File Offset: 0x00239D00
		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			if (this.fogGrid == null)
			{
				this.fogGrid = new bool[cellIndices.NumGridCells];
			}
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.fogGrid[cellIndices.CellToIndex(c)] = true;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0023B9B4 File Offset: 0x00239DB4
		private void FloodUnfogAdjacent(IntVec3 c)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult floodUnfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map))
				{
					if (intVec.Fogged(this.map))
					{
						Building edifice = intVec.GetEdifice(this.map);
						if (edifice == null || !edifice.def.MakeFog)
						{
							flag = true;
							floodUnfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
						}
						else
						{
							this.Unfog(intVec);
						}
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[j];
				if (c2.InBounds(this.map))
				{
					Building edifice2 = c2.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c2);
					}
				}
			}
			if (flag)
			{
				if (floodUnfogResult.mechanoidFound)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.ThreatBig, new TargetInfo(c, this.map, false), null, null);
				}
				else if (!floodUnfogResult.allOnScreen || floodUnfogResult.cellsUnfogged >= 600)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(c, this.map, false), null, null);
				}
			}
		}
	}
}

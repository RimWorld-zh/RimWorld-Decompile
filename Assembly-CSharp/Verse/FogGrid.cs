using RimWorld;

namespace Verse
{
	public sealed class FogGrid : IExposable
	{
		private Map map;

		public bool[] fogGrid;

		public FogGrid(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			ref bool[] arr = ref this.fogGrid;
			IntVec3 size = this.map.Size;
			int x = size.x;
			IntVec3 size2 = this.map.Size;
			ArrayExposeUtility.ExposeBoolArray(ref arr, x, size2.z, "fogGrid");
		}

		public void Unfog(IntVec3 c)
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
				if (designation != null && MineUtility.MineableInCell(c, this.map) == null)
				{
					designation.Delete();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.map.roofGrid.Drawer.SetDirty();
				}
			}
		}

		public bool IsFogged(IntVec3 c)
		{
			if (c.InBounds(this.map) && this.fogGrid != null)
			{
				return this.fogGrid[this.map.cellIndices.CellToIndex(c)];
			}
			return false;
		}

		public bool IsFogged(int index)
		{
			return this.fogGrid[index];
		}

		public void ClearAllFog()
		{
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec3 size = this.map.Size;
				if (num2 < size.x)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec3 size2 = this.map.Size;
						if (num4 < size2.z)
						{
							this.Unfog(new IntVec3(num, 0, num3));
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

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

		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
				return;
			this.FloodUnfogAdjacent(door.Position);
		}

		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			if (this.fogGrid == null)
			{
				this.fogGrid = new bool[cellIndices.NumGridCells];
			}
			foreach (IntVec3 allCell in this.map.AllCells)
			{
				this.fogGrid[cellIndices.CellToIndex(allCell)] = true;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
		}

		private void FloodUnfogAdjacent(IntVec3 c)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult floodUnfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
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
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.BadUrgent, new TargetInfo(c, this.map, false), (string)null);
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.Good, new TargetInfo(c, this.map, false), (string)null);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000428 RID: 1064
	public class PowerNetGrid
	{
		// Token: 0x04000B55 RID: 2901
		private Map map;

		// Token: 0x04000B56 RID: 2902
		private PowerNet[] netGrid;

		// Token: 0x04000B57 RID: 2903
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();

		// Token: 0x0600128D RID: 4749 RVA: 0x000A12B1 File Offset: 0x0009F6B1
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x000A12E4 File Offset: 0x0009F6E4
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000A1314 File Offset: 0x0009F714
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + newNet + " is already registered in PowerNetGrid.", false);
				this.powerNetCells.Remove(newNet);
			}
			List<IntVec3> list = new List<IntVec3>();
			this.powerNetCells.Add(newNet, list);
			for (int i = 0; i < newNet.transmitters.Count; i++)
			{
				CellRect cellRect = newNet.transmitters[i].parent.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						int num = this.map.cellIndices.CellToIndex(k, j);
						if (this.netGrid[num] != null)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Two power nets on the same cell (",
								k,
								", ",
								j,
								"). First transmitters: ",
								newNet.transmitters[0].parent.LabelCap,
								" and ",
								(!this.netGrid[num].transmitters.NullOrEmpty<CompPower>()) ? this.netGrid[num].transmitters[0].parent.LabelCap : "[none]",
								"."
							}), false);
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x000A14CC File Offset: 0x0009F8CC
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + deadNet + " does not exist in PowerNetGrid's dictionary.", false);
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					int num = this.map.cellIndices.CellToIndex(list[i]);
					if (this.netGrid[num] == deadNet)
					{
						this.netGrid[num] = null;
					}
					else
					{
						Log.Warning("Multiple nets on the same cell " + list[i] + ". This is probably a result of an earlier error.", false);
					}
				}
				this.powerNetCells.Remove(deadNet);
			}
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000A1584 File Offset: 0x0009F984
		public void DrawDebugPowerNetGrid()
		{
			if (DebugViewSettings.drawPowerNetGrid)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (this.map == Find.CurrentMap)
					{
						Rand.PushState();
						foreach (IntVec3 c in Find.CameraDriver.CurrentViewRect.ClipInsideMap(this.map))
						{
							PowerNet powerNet = this.netGrid[this.map.cellIndices.CellToIndex(c)];
							if (powerNet != null)
							{
								Rand.Seed = powerNet.GetHashCode();
								CellRenderer.RenderCell(c, Rand.Value);
							}
						}
						Rand.PopState();
					}
				}
			}
		}
	}
}

using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CAC RID: 3244
	internal class TemperatureSaveLoad
	{
		// Token: 0x04003083 RID: 12419
		private Map map;

		// Token: 0x04003084 RID: 12420
		private ushort[] tempGrid;

		// Token: 0x06004782 RID: 18306 RVA: 0x0025BB04 File Offset: 0x00259F04
		public TemperatureSaveLoad(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0025BB14 File Offset: 0x00259F14
		public void DoExposeWork()
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				int num = Mathf.RoundToInt(this.map.mapTemperature.OutdoorTemp);
				ushort num2 = this.TempFloatToShort((float)num);
				ushort[] tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					tempGrid[i] = num2;
				}
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						ushort num3 = this.TempFloatToShort(region.Room.Temperature);
						foreach (IntVec3 c2 in region.Cells)
						{
							tempGrid[this.map.cellIndices.CellToIndex(c2)] = num3;
						}
					}
				}
				arr = MapSerializeUtility.SerializeUshort(this.map, (IntVec3 c) => tempGrid[this.map.cellIndices.CellToIndex(c)]);
			}
			DataExposeUtility.ByteArray(ref arr, "temperatures");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				MapSerializeUtility.LoadUshort(arr, this.map, delegate(IntVec3 c, ushort val)
				{
					this.tempGrid[this.map.cellIndices.CellToIndex(c)] = val;
				});
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0025BCF0 File Offset: 0x0025A0F0
		public void ApplyLoadedDataToRegions()
		{
			if (this.tempGrid != null)
			{
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						region.Room.Group.Temperature = this.TempShortToFloat(this.tempGrid[cellIndices.CellToIndex(region.Cells.First<IntVec3>())]);
					}
				}
				this.tempGrid = null;
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0025BDAC File Offset: 0x0025A1AC
		private ushort TempFloatToShort(float temp)
		{
			temp = Mathf.Clamp(temp, -273.15f, 2000f);
			temp *= 16f;
			return (ushort)((int)temp + 32768);
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0025BDE8 File Offset: 0x0025A1E8
		private float TempShortToFloat(ushort temp)
		{
			return ((float)temp - 32768f) / 16f;
		}
	}
}

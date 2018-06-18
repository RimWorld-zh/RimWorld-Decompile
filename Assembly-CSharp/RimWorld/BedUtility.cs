using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A0 RID: 1696
	public static class BedUtility
	{
		// Token: 0x0600240B RID: 9227 RVA: 0x00135F78 File Offset: 0x00134378
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00135F94 File Offset: 0x00134394
		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			IntVec3 result;
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get sleeping slot pos with index ",
					index,
					", but there are only ",
					sleepingSlotsCount,
					" sleeping slots available."
				}), false);
				result = bedCenter;
			}
			else
			{
				CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
				if (bedRot == Rot4.North)
				{
					result = new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.minZ);
				}
				else if (bedRot == Rot4.East)
				{
					result = new IntVec3(cellRect.minX, bedCenter.y, cellRect.maxZ - index);
				}
				else if (bedRot == Rot4.South)
				{
					result = new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.maxZ);
				}
				else
				{
					result = new IntVec3(cellRect.maxX, bedCenter.y, cellRect.maxZ - index);
				}
			}
			return result;
		}
	}
}

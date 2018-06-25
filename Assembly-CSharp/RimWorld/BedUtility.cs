using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069E RID: 1694
	public static class BedUtility
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x00136210 File Offset: 0x00134610
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0013622C File Offset: 0x0013462C
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

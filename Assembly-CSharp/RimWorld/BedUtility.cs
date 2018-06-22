using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069C RID: 1692
	public static class BedUtility
	{
		// Token: 0x06002403 RID: 9219 RVA: 0x001360C0 File Offset: 0x001344C0
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x001360DC File Offset: 0x001344DC
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

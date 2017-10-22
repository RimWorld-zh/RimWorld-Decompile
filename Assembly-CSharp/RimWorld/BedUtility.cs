using Verse;

namespace RimWorld
{
	public static class BedUtility
	{
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			IntVec3 result;
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error("Tried to get sleeping slot pos with index " + index + ", but there are only " + sleepingSlotsCount + " sleeping slots available.");
				result = bedCenter;
			}
			else
			{
				CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
				result = ((!(bedRot == Rot4.North)) ? ((!(bedRot == Rot4.East)) ? ((!(bedRot == Rot4.South)) ? new IntVec3(cellRect.maxX, bedCenter.y, cellRect.maxZ - index) : new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.maxZ)) : new IntVec3(cellRect.minX, bedCenter.y, cellRect.maxZ - index)) : new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.minZ));
			}
			return result;
		}
	}
}

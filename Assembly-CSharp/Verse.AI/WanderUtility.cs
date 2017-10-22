namespace Verse.AI
{
	public static class WanderUtility
	{
		public static IntVec3 BestCloseWanderRoot(IntVec3 trueWanderRoot, Pawn pawn)
		{
			int num = 0;
			IntVec3 result;
			while (true)
			{
				if (num < 50)
				{
					IntVec3 intVec = (num >= 8) ? (trueWanderRoot + GenRadial.RadialPattern[num - 8 + 1] * 7) : (trueWanderRoot + GenRadial.RadialPattern[num]);
					if (intVec.InBounds(pawn.Map) && intVec.Walkable(pawn.Map) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn))
					{
						result = intVec;
						break;
					}
					num++;
					continue;
				}
				result = IntVec3.Invalid;
				break;
			}
			return result;
		}

		public static bool InSameRoom(IntVec3 locA, IntVec3 locB, Map map)
		{
			Room room = locA.GetRoom(map, RegionType.Set_All);
			return room == null || room == locB.GetRoom(map, RegionType.Set_All);
		}
	}
}

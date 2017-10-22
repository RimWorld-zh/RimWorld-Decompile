namespace Verse.AI
{
	public static class WanderRoomUtility
	{
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			if (room != null && room.RegionType != RegionType.Portal)
			{
				return WanderUtility.InSameRoom(root, loc, pawn.Map);
			}
			return true;
		}
	}
}

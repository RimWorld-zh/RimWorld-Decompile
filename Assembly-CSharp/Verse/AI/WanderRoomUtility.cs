using System;

namespace Verse.AI
{
	// Token: 0x02000AD5 RID: 2773
	public static class WanderRoomUtility
	{
		// Token: 0x06003D88 RID: 15752 RVA: 0x00205D64 File Offset: 0x00204164
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			return room == null || room.RegionType == RegionType.Portal || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}

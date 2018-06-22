using System;

namespace Verse.AI
{
	// Token: 0x02000AD1 RID: 2769
	public static class WanderRoomUtility
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x00206088 File Offset: 0x00204488
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			return room == null || room.RegionType == RegionType.Portal || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}

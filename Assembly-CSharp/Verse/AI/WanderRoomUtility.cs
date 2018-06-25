using System;

namespace Verse.AI
{
	// Token: 0x02000AD3 RID: 2771
	public static class WanderRoomUtility
	{
		// Token: 0x06003D87 RID: 15751 RVA: 0x002061B4 File Offset: 0x002045B4
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map, RegionType.Set_Passable);
			return room == null || room.RegionType == RegionType.Portal || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098C RID: 2444
	public static class InventoryCalculatorsUtility
	{
		// Token: 0x060036F4 RID: 14068 RVA: 0x001D5CBC File Offset: 0x001D40BC
		public static bool ShouldIgnoreInventoryOf(Pawn pawn, IgnorePawnsInventoryMode ignoreMode)
		{
			bool result;
			switch (ignoreMode)
			{
			case IgnorePawnsInventoryMode.Ignore:
				result = true;
				break;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload:
				result = (pawn.Spawned && pawn.inventory.UnloadEverything);
				break;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn:
				result = ((pawn.Spawned && pawn.inventory.UnloadEverything) || Dialog_FormCaravan.CanListInventorySeparately(pawn));
				break;
			case IgnorePawnsInventoryMode.DontIgnore:
				result = false;
				break;
			default:
				throw new NotImplementedException("IgnorePawnsInventoryMode");
			}
			return result;
		}
	}
}

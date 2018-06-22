using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000988 RID: 2440
	public static class InventoryCalculatorsUtility
	{
		// Token: 0x060036ED RID: 14061 RVA: 0x001D5EB8 File Offset: 0x001D42B8
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

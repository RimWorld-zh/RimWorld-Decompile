using System;
using Verse;

namespace RimWorld
{
	public static class InventoryCalculatorsUtility
	{
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

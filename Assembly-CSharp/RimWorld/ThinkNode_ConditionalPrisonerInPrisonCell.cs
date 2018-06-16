using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BC RID: 444
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x0600092F RID: 2351 RVA: 0x00055E74 File Offset: 0x00054274
		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (!pawn.IsPrisoner)
			{
				result = false;
			}
			else
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				result = (room != null && room.isPrisonCell);
			}
			return result;
		}
	}
}

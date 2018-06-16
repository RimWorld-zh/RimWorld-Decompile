using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A06 RID: 2566
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		// Token: 0x06003980 RID: 14720 RVA: 0x001E783C File Offset: 0x001E5C3C
		public override void DoAction(Transition trans)
		{
			LordToil_Travel lordToil_Travel = (LordToil_Travel)trans.target;
			if (!lordToil_Travel.HasDestination())
			{
				Pawn pawn = lordToil_Travel.lord.ownedPawns.RandomElement<Pawn>();
				IntVec3 destination;
				if (!CellFinder.TryFindRandomPawnExitCell(pawn, out destination))
				{
					RCellFinder.TryFindRandomPawnEntryCell(out destination, pawn.Map, 0f, null);
				}
				lordToil_Travel.SetDestination(destination);
			}
		}
	}
}

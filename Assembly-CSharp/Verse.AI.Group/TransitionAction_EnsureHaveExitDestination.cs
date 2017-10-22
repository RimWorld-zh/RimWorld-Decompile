using RimWorld;
using System;

namespace Verse.AI.Group
{
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		public override void DoAction(Transition trans)
		{
			LordToil_Travel lordToil_Travel = (LordToil_Travel)trans.target;
			if (!lordToil_Travel.HasDestination())
			{
				Pawn pawn = lordToil_Travel.lord.ownedPawns.RandomElement();
				IntVec3 destination = default(IntVec3);
				if (!CellFinder.TryFindRandomPawnExitCell(pawn, out destination))
				{
					RCellFinder.TryFindRandomPawnEntryCell(out destination, pawn.Map, 0f, (Predicate<IntVec3>)null);
				}
				lordToil_Travel.SetDestination(destination);
			}
		}
	}
}

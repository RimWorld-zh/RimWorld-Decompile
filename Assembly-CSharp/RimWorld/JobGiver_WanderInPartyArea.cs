using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_WanderInPartyArea : JobGiver_Wander
	{
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 intVec = default(IntVec3);
			return PartyUtility.TryFindRandomCellInPartyArea(pawn, out intVec) ? intVec : IntVec3.Invalid;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}
}

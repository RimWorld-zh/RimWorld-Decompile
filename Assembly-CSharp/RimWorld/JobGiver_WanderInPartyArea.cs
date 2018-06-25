using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_WanderInPartyArea : JobGiver_Wander
	{
		public JobGiver_WanderInPartyArea()
		{
		}

		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (!PartyUtility.TryFindRandomCellInPartyArea(pawn, out intVec))
			{
				result = IntVec3.Invalid;
			}
			else
			{
				result = intVec;
			}
			return result;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}
}

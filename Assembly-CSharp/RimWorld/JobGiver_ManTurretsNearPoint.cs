using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		public JobGiver_ManTurretsNearPoint()
		{
		}

		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}

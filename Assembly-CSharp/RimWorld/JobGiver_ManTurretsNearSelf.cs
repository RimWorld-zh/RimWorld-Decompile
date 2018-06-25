using System;
using Verse;

namespace RimWorld
{
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		public JobGiver_ManTurretsNearSelf()
		{
		}

		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}

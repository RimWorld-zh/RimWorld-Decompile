using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		protected LordJob_VoluntarilyJoinable()
		{
		}

		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}
	}
}

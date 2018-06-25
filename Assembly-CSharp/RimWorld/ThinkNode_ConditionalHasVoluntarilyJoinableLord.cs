using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_ConditionalHasVoluntarilyJoinableLord : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalHasVoluntarilyJoinableLord()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_VoluntarilyJoinable;
		}
	}
}

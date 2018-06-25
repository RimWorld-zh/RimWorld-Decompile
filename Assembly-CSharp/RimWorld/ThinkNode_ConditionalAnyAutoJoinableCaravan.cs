using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyAutoJoinableCaravan : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalAnyAutoJoinableCaravan()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null;
		}
	}
}

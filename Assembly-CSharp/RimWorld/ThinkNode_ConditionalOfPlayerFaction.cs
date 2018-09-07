using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalOfPlayerFaction()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}

using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}

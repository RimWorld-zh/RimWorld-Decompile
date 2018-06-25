using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalAnyEnemyInHostileMap()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (!pawn.Spawned)
			{
				result = false;
			}
			else
			{
				Map map = pawn.Map;
				result = (!map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map));
			}
			return result;
		}
	}
}

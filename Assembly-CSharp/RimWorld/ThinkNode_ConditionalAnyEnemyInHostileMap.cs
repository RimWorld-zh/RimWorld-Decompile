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
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map);
		}
	}
}

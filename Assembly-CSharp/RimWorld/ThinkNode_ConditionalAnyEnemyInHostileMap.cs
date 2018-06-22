using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001EB RID: 491
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x00056CBC File Offset: 0x000550BC
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

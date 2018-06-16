using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A9 RID: 169
	public static class SelfDefenseUtility
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x00031414 File Offset: 0x0002F814
		public static bool ShouldStartFleeing(Pawn pawn)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
			for (int i = 0; i < list.Count; i++)
			{
				if (SelfDefenseUtility.ShouldFleeFrom(list[i], pawn, true, false))
				{
					return true;
				}
			}
			bool foundThreat = false;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.portal == null || reg.portal.Open, delegate(Region reg)
			{
				List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int j = 0; j < list2.Count; j++)
				{
					if (SelfDefenseUtility.ShouldFleeFrom(list2[j], pawn, true, true))
					{
						foundThreat = true;
						break;
					}
				}
				return foundThreat;
			}, 9, RegionType.Set_Passable);
			return foundThreat;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000314E4 File Offset: 0x0002F8E4
		public static bool ShouldFleeFrom(Thing t, Pawn pawn, bool checkDistance, bool checkLOS)
		{
			bool result;
			if (t == pawn || (checkDistance && !t.Position.InHorDistOf(pawn.Position, 8f)))
			{
				result = false;
			}
			else if (t.def.alwaysFlee)
			{
				result = true;
			}
			else if (!t.HostileTo(pawn))
			{
				result = false;
			}
			else
			{
				IAttackTarget attackTarget = t as IAttackTarget;
				result = (attackTarget != null && !attackTarget.ThreatDisabled(pawn) && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map, false, null, 0, 0)));
			}
			return result;
		}

		// Token: 0x04000273 RID: 627
		public const float FleeWhenDistToHostileLessThan = 8f;
	}
}

using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SelfDefenseUtility
	{
		public const float FleeWhenDistToHostileLessThan = 8f;

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
				int num = 0;
				while (num < list2.Count)
				{
					if (!SelfDefenseUtility.ShouldFleeFrom(list2[num], pawn, true, true))
					{
						num++;
						continue;
					}
					foundThreat = true;
					break;
				}
				return foundThreat;
			}, 9, RegionType.Set_Passable);
			return foundThreat;
		}

		public static bool ShouldFleeFrom(Thing t, Pawn pawn, bool checkDistance, bool checkLOS)
		{
			if (t != pawn && (!checkDistance || t.Position.InHorDistOf(pawn.Position, 8f)))
			{
				if (t.def.alwaysFlee)
				{
					return true;
				}
				if (!t.HostileTo(pawn))
				{
					return false;
				}
				IAttackTarget attackTarget = t as IAttackTarget;
				if (attackTarget != null && !attackTarget.ThreatDisabled() && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map, false, null, 0, 0)))
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}

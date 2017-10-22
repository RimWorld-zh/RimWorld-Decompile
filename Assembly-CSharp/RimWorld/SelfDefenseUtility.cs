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
			int num = 0;
			bool result;
			bool foundThreat;
			while (true)
			{
				if (num < list.Count)
				{
					if (SelfDefenseUtility.ShouldFleeFrom(list[num], pawn, true, false))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				foundThreat = false;
				Region region = pawn.GetRegion(RegionType.Set_Passable);
				if (region == null)
				{
					result = false;
				}
				else
				{
					RegionTraverser.BreadthFirstTraverse(region, (RegionEntryPredicate)((Region from, Region reg) => reg.portal == null || reg.portal.Open), (RegionProcessor)delegate(Region reg)
					{
						List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
						int num2 = 0;
						while (num2 < list2.Count)
						{
							if (!SelfDefenseUtility.ShouldFleeFrom(list2[num2], pawn, true, true))
							{
								num2++;
								continue;
							}
							foundThreat = true;
							break;
						}
						return foundThreat;
					}, 9, RegionType.Set_Passable);
					result = foundThreat;
				}
				break;
			}
			return result;
		}

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
				result = ((byte)((attackTarget != null && !attackTarget.ThreatDisabled() && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map, false, null, 0, 0))) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SelfDefenseUtility
	{
		public const float FleeWhenDistToHostileLessThan = 8f;

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

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

		[CompilerGenerated]
		private static bool <ShouldStartFleeing>m__0(Region from, Region reg)
		{
			return reg.portal == null || reg.portal.Open;
		}

		[CompilerGenerated]
		private sealed class <ShouldStartFleeing>c__AnonStorey0
		{
			internal Pawn pawn;

			internal bool foundThreat;

			public <ShouldStartFleeing>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region reg)
			{
				List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (SelfDefenseUtility.ShouldFleeFrom(list[i], this.pawn, true, true))
					{
						this.foundThreat = true;
						break;
					}
				}
				return this.foundThreat;
			}
		}
	}
}

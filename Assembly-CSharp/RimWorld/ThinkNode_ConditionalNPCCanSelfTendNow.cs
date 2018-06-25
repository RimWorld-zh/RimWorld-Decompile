using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_ConditionalNPCCanSelfTendNow : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalNPCCanSelfTendNow()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (!pawn.health.hediffSet.hediffs.Any<Hediff>())
			{
				result = false;
			}
			else if (!pawn.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (pawn.Faction == Faction.OfPlayer)
			{
				result = false;
			}
			else if (pawn.Faction != null && pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				result = false;
			}
			else if (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner)
			{
				result = false;
			}
			else if (Find.TickManager.TicksGame < pawn.mindState.lastHarmTick + 300)
			{
				result = false;
			}
			else if (Find.TickManager.TicksGame < pawn.mindState.lastEngageTargetTick + 300)
			{
				result = false;
			}
			else if (Find.TickManager.TicksGame < pawn.mindState.lastSelfTendTick + 300)
			{
				result = false;
			}
			else
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowSelfTend)
				{
					result = false;
				}
				else if (!pawn.health.HasHediffsNeedingTend(false))
				{
					result = false;
				}
				else
				{
					if (pawn.Faction != null)
					{
						bool foundActiveThreat = false;
						RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, RegionTraverser.PassAll, delegate(Region x)
						{
							List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
							for (int i = 0; i < list.Count; i++)
							{
								if (GenHostility.IsActiveThreatTo((IAttackTarget)list[i], pawn.Faction))
								{
									foundActiveThreat = true;
									break;
								}
							}
							return foundActiveThreat;
						}, 5, RegionType.Set_Passable);
						if (foundActiveThreat)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <Satisfied>c__AnonStorey0
		{
			internal Pawn pawn;

			public <Satisfied>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <Satisfied>c__AnonStorey1
		{
			internal bool foundActiveThreat;

			internal ThinkNode_ConditionalNPCCanSelfTendNow.<Satisfied>c__AnonStorey0 <>f__ref$0;

			public <Satisfied>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (GenHostility.IsActiveThreatTo((IAttackTarget)list[i], this.<>f__ref$0.pawn.Faction))
					{
						this.foundActiveThreat = true;
						break;
					}
				}
				return this.foundActiveThreat;
			}
		}
	}
}

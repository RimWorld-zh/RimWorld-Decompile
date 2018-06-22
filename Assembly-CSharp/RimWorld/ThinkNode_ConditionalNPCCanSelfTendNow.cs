using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001EC RID: 492
	public class ThinkNode_ConditionalNPCCanSelfTendNow : ThinkNode_Conditional
	{
		// Token: 0x06000994 RID: 2452 RVA: 0x00056D28 File Offset: 0x00055128
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
	}
}

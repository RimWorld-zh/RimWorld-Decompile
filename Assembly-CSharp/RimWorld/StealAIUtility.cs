using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000DA RID: 218
	public static class StealAIUtility
	{
		// Token: 0x040002AB RID: 683
		private const float MinMarketValueToTake = 320f;

		// Token: 0x040002AC RID: 684
		private static readonly FloatRange StealThresholdValuePerCombatPowerRange = new FloatRange(2f, 10f);

		// Token: 0x040002AD RID: 685
		private const float MinCombatPowerPerPawn = 100f;

		// Token: 0x040002AE RID: 686
		private static List<Thing> tmpToSteal = new List<Thing>();

		// Token: 0x060004CA RID: 1226 RVA: 0x00035994 File Offset: 0x00033D94
		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			bool result;
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				result = false;
			}
			else if ((thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false))) || (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false))))
			{
				item = null;
				result = false;
			}
			else
			{
				Predicate<Thing> validator = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
				item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, (Thing x) => StealAIUtility.GetValue(x), 15, 15);
				if (item != null && StealAIUtility.GetValue(item) < 320f)
				{
					item = null;
				}
				result = (item != null);
			}
			return result;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00035AC4 File Offset: 0x00033EC4
		public static float TotalMarketValueAround(List<Pawn> pawns)
		{
			float num = 0f;
			StealAIUtility.tmpToSteal.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i].Spawned)
				{
					Thing thing;
					if (StealAIUtility.TryFindBestItemToSteal(pawns[i].Position, pawns[i].Map, 7f, out thing, pawns[i], StealAIUtility.tmpToSteal))
					{
						num += StealAIUtility.GetValue(thing);
						StealAIUtility.tmpToSteal.Add(thing);
					}
				}
			}
			StealAIUtility.tmpToSteal.Clear();
			return num;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00035B70 File Offset: 0x00033F70
		public static float StartStealingMarketValueThreshold(Lord lord)
		{
			Rand.PushState();
			Rand.Seed = lord.loadID;
			float randomInRange = StealAIUtility.StealThresholdValuePerCombatPowerRange.RandomInRange;
			Rand.PopState();
			float num = 0f;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				num += Mathf.Max(lord.ownedPawns[i].kindDef.combatPower, 100f);
			}
			return num * randomInRange;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00035BF8 File Offset: 0x00033FF8
		public static float GetValue(Thing thing)
		{
			return thing.MarketValue * (float)thing.stackCount;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class StealAIUtility
	{
		private const float MinMarketValueToTake = 320f;

		private static readonly FloatRange StealThresholdValuePerCombatPowerRange = new FloatRange(2f, 10f);

		private const float MinCombatPowerPerPawn = 100f;

		private static List<Thing> tmpToSteal = new List<Thing>();

		[CompilerGenerated]
		private static Func<Thing, float> _003C_003Ef__mg_0024cache0;

		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				return false;
			}
			if (thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false)))
			{
				goto IL_009d;
			}
			if (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false)))
				goto IL_009d;
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (thief != null && !thief.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (disallowed != null && disallowed.Contains(t))
				{
					return false;
				}
				if (!t.def.stealable)
				{
					return false;
				}
				if (t.IsBurning())
				{
					return false;
				}
				return true;
			};
			item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, StealAIUtility.GetValue, 15, 15);
			if (item != null && StealAIUtility.GetValue(item) < 320.0)
			{
				item = null;
			}
			return item != null;
			IL_009d:
			item = null;
			return false;
		}

		public static float TotalMarketValueAround(List<Pawn> pawns)
		{
			float num = 0f;
			StealAIUtility.tmpToSteal.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Thing thing = default(Thing);
				if (pawns[i].Spawned && StealAIUtility.TryFindBestItemToSteal(pawns[i].Position, pawns[i].Map, 7f, out thing, pawns[i], StealAIUtility.tmpToSteal))
				{
					num += StealAIUtility.GetValue(thing);
					StealAIUtility.tmpToSteal.Add(thing);
				}
			}
			StealAIUtility.tmpToSteal.Clear();
			return num;
		}

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

		public static float GetValue(Thing thing)
		{
			return thing.MarketValue * (float)thing.stackCount;
		}
	}
}

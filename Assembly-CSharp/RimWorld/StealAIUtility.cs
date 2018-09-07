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
		private static Func<Thing, float> <>f__mg$cache0;

		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				return false;
			}
			if ((thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false))) || (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false))))
			{
				item = null;
				return false;
			}
			Predicate<Thing> predicate = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable);
			PathEndMode peMode = PathEndMode.ClosestTouch;
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false);
			Predicate<Thing> validator = predicate;
			if (StealAIUtility.<>f__mg$cache0 == null)
			{
				StealAIUtility.<>f__mg$cache0 = new Func<Thing, float>(StealAIUtility.GetValue);
			}
			item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, thingReq, peMode, traverseParams, maxDist, validator, StealAIUtility.<>f__mg$cache0, 15, 15);
			if (item != null && StealAIUtility.GetValue(item) < 320f)
			{
				item = null;
			}
			return item != null;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static StealAIUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <TryFindBestItemToSteal>c__AnonStorey0
		{
			internal Pawn thief;

			internal List<Thing> disallowed;

			public <TryFindBestItemToSteal>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return (this.thief == null || this.thief.CanReserve(t, 1, -1, null, false)) && (this.disallowed == null || !this.disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
			}
		}
	}
}

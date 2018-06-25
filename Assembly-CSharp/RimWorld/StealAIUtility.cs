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
		private static Func<Thing, float> <>f__am$cache0;

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
		private static float <TryFindBestItemToSteal>m__0(Thing x)
		{
			return StealAIUtility.GetValue(x);
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

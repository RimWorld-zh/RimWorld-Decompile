using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		private const int MinDistToHumanlike = 15;

		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (((Pawn)t).RaceProps != pawn.RaceProps || t == pawn)
				{
					return false;
				}
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
				if (t.Position.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return false;
				}
				if (Rand.Value < 0.5f)
				{
					return false;
				}
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.RaceProps.Humanlike && (pawn2.Position - t.Position).LengthHorizontalSquared < 225)
					{
						return false;
					}
				}
				return true;
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return thing.Position;
			}
			return pawn.Position;
		}

		[CompilerGenerated]
		private sealed class <GetWanderRoot>c__AnonStorey0
		{
			internal Pawn pawn;

			public <GetWanderRoot>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				if (((Pawn)t).RaceProps != this.pawn.RaceProps || t == this.pawn)
				{
					return false;
				}
				if (t.Faction != this.pawn.Faction)
				{
					return false;
				}
				if (t.Position.IsForbidden(this.pawn))
				{
					return false;
				}
				if (!this.pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return false;
				}
				if (Rand.Value < 0.5f)
				{
					return false;
				}
				List<Pawn> allPawnsSpawned = this.pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn = allPawnsSpawned[i];
					if (pawn.RaceProps.Humanlike && (pawn.Position - t.Position).LengthHorizontalSquared < 225)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}

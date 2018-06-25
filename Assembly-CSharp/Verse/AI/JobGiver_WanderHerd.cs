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
				bool result;
				if (((Pawn)t).RaceProps != pawn.RaceProps || t == pawn)
				{
					result = false;
				}
				else if (t.Faction != pawn.Faction)
				{
					result = false;
				}
				else if (t.Position.IsForbidden(pawn))
				{
					result = false;
				}
				else if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = false;
				}
				else if (Rand.Value < 0.5f)
				{
					result = false;
				}
				else
				{
					List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
					for (int i = 0; i < allPawnsSpawned.Count; i++)
					{
						Pawn pawn2 = allPawnsSpawned[i];
						if (pawn2.RaceProps.Humanlike && (pawn2.Position - t.Position).LengthHorizontalSquared < 225)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false);
			IntVec3 position;
			if (thing != null)
			{
				position = thing.Position;
			}
			else
			{
				position = pawn.Position;
			}
			return position;
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
				bool result;
				if (((Pawn)t).RaceProps != this.pawn.RaceProps || t == this.pawn)
				{
					result = false;
				}
				else if (t.Faction != this.pawn.Faction)
				{
					result = false;
				}
				else if (t.Position.IsForbidden(this.pawn))
				{
					result = false;
				}
				else if (!this.pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = false;
				}
				else if (Rand.Value < 0.5f)
				{
					result = false;
				}
				else
				{
					List<Pawn> allPawnsSpawned = this.pawn.Map.mapPawns.AllPawnsSpawned;
					for (int i = 0; i < allPawnsSpawned.Count; i++)
					{
						Pawn pawn = allPawnsSpawned[i];
						if (pawn.RaceProps.Humanlike && (pawn.Position - t.Position).LengthHorizontalSquared < 225)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}
	}
}

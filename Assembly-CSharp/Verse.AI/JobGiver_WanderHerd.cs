using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		private const int MinDistToHumanlike = 15;

		public JobGiver_WanderHerd()
		{
			base.wanderRadius = 5f;
			base.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
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
				else if (Rand.Value < 0.5)
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
							goto IL_00fa;
					}
					result = true;
				}
				goto IL_0119;
				IL_00fa:
				result = false;
				goto IL_0119;
				IL_0119:
				return result;
			};
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false);
			return (thing == null) ? pawn.Position : thing.Position;
		}
	}
}

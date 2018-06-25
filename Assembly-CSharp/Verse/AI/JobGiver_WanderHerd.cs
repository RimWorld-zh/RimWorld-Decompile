using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD4 RID: 2772
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x040026C1 RID: 9921
		private const int MinDistToHumanlike = 15;

		// Token: 0x06003D88 RID: 15752 RVA: 0x002061FD File Offset: 0x002045FD
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00206224 File Offset: 0x00204624
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
	}
}

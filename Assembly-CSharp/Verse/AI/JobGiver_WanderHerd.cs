using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD6 RID: 2774
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x06003D89 RID: 15753 RVA: 0x00205DAD File Offset: 0x002041AD
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x00205DD4 File Offset: 0x002041D4
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

		// Token: 0x040026C5 RID: 9925
		private const int MinDistToHumanlike = 15;
	}
}

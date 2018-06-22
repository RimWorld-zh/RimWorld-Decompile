using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000C2 RID: 194
	public class JobGiver_PackAnimalFollowColonists : ThinkNode_JobGiver
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x00033AF8 File Offset: 0x00031EF8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawnToFollow = JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn);
			Job result;
			if (pawnToFollow == null)
			{
				result = null;
			}
			else if (pawnToFollow.Position.InHorDistOf(pawn.Position, 10f) && pawnToFollow.Position.WithinRegions(pawn.Position, pawn.Map, 5, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.Follow, pawnToFollow)
				{
					locomotionUrgency = LocomotionUrgency.Walk,
					checkOverrideOnExpire = true,
					expiryInterval = 120
				};
			}
			return result;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00033B98 File Offset: 0x00031F98
		public static Pawn GetPawnToFollow(Pawn forPawn)
		{
			Pawn result;
			if (!forPawn.RaceProps.packAnimal || forPawn.inventory.UnloadEverything || MassUtility.IsOverEncumbered(forPawn))
			{
				result = null;
			}
			else
			{
				Lord lord = forPawn.GetLord();
				if (lord == null)
				{
					result = null;
				}
				else
				{
					List<Pawn> ownedPawns = lord.ownedPawns;
					for (int i = 0; i < ownedPawns.Count; i++)
					{
						Pawn pawn = ownedPawns[i];
						if (pawn != forPawn && CaravanUtility.IsOwner(pawn, forPawn.Faction) && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PrepareCaravan_GatherItems && forPawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							JobDriver_PrepareCaravan_GatherItems jobDriver_PrepareCaravan_GatherItems = (JobDriver_PrepareCaravan_GatherItems)pawn.jobs.curDriver;
							if (jobDriver_PrepareCaravan_GatherItems.Carrier == forPawn)
							{
								return pawn;
							}
						}
					}
					Pawn pawn2 = null;
					for (int j = 0; j < ownedPawns.Count; j++)
					{
						Pawn pawn3 = ownedPawns[j];
						if (pawn3 != forPawn && CaravanUtility.IsOwner(pawn3, forPawn.Faction) && pawn3.CurJob != null && pawn3.CurJob.def == JobDefOf.PrepareCaravan_GatherItems && forPawn.CanReach(pawn3, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && (pawn2 == null || forPawn.Position.DistanceToSquared(pawn3.Position) < forPawn.Position.DistanceToSquared(pawn2.Position)))
						{
							pawn2 = pawn3;
						}
					}
					result = pawn2;
				}
			}
			return result;
		}

		// Token: 0x04000299 RID: 665
		private const int MaxDistanceToPawnToFollow = 10;
	}
}

using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000C5 RID: 197
	public class JobGiver_PrepareCaravan_GatherPawns : ThinkNode_JobGiver
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0003409C File Offset: 0x0003249C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = this.FindPawn(pawn);
			Job result;
			if (pawn2 == null)
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.PrepareCaravan_GatherPawns, pawn2)
				{
					lord = pawn.GetLord()
				};
			}
			return result;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000340E4 File Offset: 0x000324E4
		private Pawn FindPawn(Pawn pawn)
		{
			Pawn result;
			if (pawn.mindState.duty.pawnsToGather == PawnsToGather.None)
			{
				result = null;
			}
			else
			{
				float num = 0f;
				Pawn pawn2 = null;
				Lord lord = pawn.GetLord();
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn3 = lord.ownedPawns[i];
					if (pawn3 != pawn)
					{
						if (!pawn3.IsColonist)
						{
							if (pawn.mindState.duty.pawnsToGather != PawnsToGather.Slaves || !pawn3.RaceProps.Animal)
							{
								if (pawn.mindState.duty.pawnsToGather != PawnsToGather.Animals || pawn3.RaceProps.Animal)
								{
									if (!GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(pawn3))
									{
										float num2 = (float)pawn.Position.DistanceToSquared(pawn3.Position);
										if (pawn2 == null || num2 < num)
										{
											if (pawn.CanReserveAndReach(pawn3, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
											{
												pawn2 = pawn3;
												num = num2;
											}
										}
									}
								}
							}
						}
					}
				}
				result = pawn2;
			}
			return result;
		}
	}
}

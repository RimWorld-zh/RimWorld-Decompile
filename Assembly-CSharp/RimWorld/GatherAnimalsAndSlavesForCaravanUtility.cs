using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000181 RID: 385
	public static class GatherAnimalsAndSlavesForCaravanUtility
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x0004DFE0 File Offset: 0x0004C3E0
		public static bool IsFollowingAnyone(Pawn p)
		{
			return p.mindState.duty.focus.HasThing;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0004E00A File Offset: 0x0004C40A
		public static void SetFollower(Pawn p, Pawn follower)
		{
			p.mindState.duty.focus = follower;
			p.mindState.duty.radius = 10f;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0004E038 File Offset: 0x0004C438
		public static void CheckArrived(Lord lord, IntVec3 meetingPoint, string memo, Predicate<Pawn> shouldCheckIfArrived, Predicate<Pawn> extraValidator = null)
		{
			bool flag = true;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (shouldCheckIfArrived(pawn))
				{
					if (!pawn.Position.InHorDistOf(meetingPoint, 10f) || !pawn.CanReach(meetingPoint, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) || (extraValidator != null && !extraValidator(pawn)))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				lord.ReceiveMemo(memo);
			}
		}
	}
}

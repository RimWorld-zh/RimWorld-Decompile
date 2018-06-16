using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B7 RID: 439
	public class ThinkNode_JoinVoluntarilyJoinableLord : ThinkNode_Priority
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x000559FC File Offset: 0x00053DFC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JoinVoluntarilyJoinableLord thinkNode_JoinVoluntarilyJoinableLord = (ThinkNode_JoinVoluntarilyJoinableLord)base.DeepCopy(resolve);
			thinkNode_JoinVoluntarilyJoinableLord.dutyHook = this.dutyHook;
			return thinkNode_JoinVoluntarilyJoinableLord;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00055A2C File Offset: 0x00053E2C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			this.CheckLeaveCurrentVoluntarilyJoinableLord(pawn);
			this.JoinVoluntarilyJoinableLord(pawn);
			if (pawn.GetLord() != null)
			{
				if (pawn.mindState.duty == null || pawn.mindState.duty.def.hook == this.dutyHook)
				{
					return base.TryIssueJobPackage(pawn, jobParams);
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00055AA0 File Offset: 0x00053EA0
		private void CheckLeaveCurrentVoluntarilyJoinableLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable = lord.LordJob as LordJob_VoluntarilyJoinable;
				if (lordJob_VoluntarilyJoinable != null)
				{
					if (lordJob_VoluntarilyJoinable.VoluntaryJoinPriorityFor(pawn) <= 0f)
					{
						lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily);
					}
				}
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00055AF0 File Offset: 0x00053EF0
		private void JoinVoluntarilyJoinableLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			Lord lord2 = null;
			float num = 0f;
			if (lord != null)
			{
				LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable = lord.LordJob as LordJob_VoluntarilyJoinable;
				if (lordJob_VoluntarilyJoinable == null)
				{
					return;
				}
				lord2 = lord;
				num = lordJob_VoluntarilyJoinable.VoluntaryJoinPriorityFor(pawn);
			}
			List<Lord> lords = pawn.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_VoluntarilyJoinable lordJob_VoluntarilyJoinable2 = lords[i].LordJob as LordJob_VoluntarilyJoinable;
				if (lordJob_VoluntarilyJoinable2 != null)
				{
					if (lords[i].CurLordToil.VoluntaryJoinDutyHookFor(pawn) == this.dutyHook)
					{
						float num2 = lordJob_VoluntarilyJoinable2.VoluntaryJoinPriorityFor(pawn);
						if (num2 > 0f)
						{
							if (lord2 == null || num2 > num)
							{
								lord2 = lords[i];
								num = num2;
							}
						}
					}
				}
			}
			if (lord2 != null && lord != lord2)
			{
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily);
				}
				lord2.AddPawn(pawn);
			}
		}

		// Token: 0x040003DB RID: 987
		public ThinkTreeDutyHook dutyHook;
	}
}

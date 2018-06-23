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
		// Token: 0x040003D9 RID: 985
		public ThinkTreeDutyHook dutyHook;

		// Token: 0x06000920 RID: 2336 RVA: 0x00055A10 File Offset: 0x00053E10
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JoinVoluntarilyJoinableLord thinkNode_JoinVoluntarilyJoinableLord = (ThinkNode_JoinVoluntarilyJoinableLord)base.DeepCopy(resolve);
			thinkNode_JoinVoluntarilyJoinableLord.dutyHook = this.dutyHook;
			return thinkNode_JoinVoluntarilyJoinableLord;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00055A40 File Offset: 0x00053E40
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

		// Token: 0x06000922 RID: 2338 RVA: 0x00055AB4 File Offset: 0x00053EB4
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

		// Token: 0x06000923 RID: 2339 RVA: 0x00055B04 File Offset: 0x00053F04
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
	}
}

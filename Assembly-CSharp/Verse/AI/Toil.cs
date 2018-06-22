using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A44 RID: 2628
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06003A7F RID: 14975 RVA: 0x001F0110 File Offset: 0x001EE510
		public void Cleanup(int myIndex, JobDriver jobDriver)
		{
			if (this.finishActions != null)
			{
				for (int i = 0; i < this.finishActions.Count; i++)
				{
					try
					{
						this.finishActions[i]();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn ",
							this.actor.ToStringSafe<Pawn>(),
							" threw exception while executing toil's finish action (",
							i,
							"), jobDriver=",
							jobDriver.ToStringSafe<JobDriver>(),
							", job=",
							jobDriver.job.ToStringSafe<Job>(),
							", toilIndex=",
							myIndex,
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x001F01FC File Offset: 0x001EE5FC
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x001F0218 File Offset: 0x001EE618
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add(delegate
			{
				JobCondition result;
				if (newFailCondition())
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x001F024A File Offset: 0x001EE64A
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x001F0259 File Offset: 0x001EE659
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x001F027E File Offset: 0x001EE67E
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x001F02A3 File Offset: 0x001EE6A3
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x0400252D RID: 9517
		public Pawn actor;

		// Token: 0x0400252E RID: 9518
		public Action initAction = null;

		// Token: 0x0400252F RID: 9519
		public Action tickAction = null;

		// Token: 0x04002530 RID: 9520
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x04002531 RID: 9521
		public List<Action> preInitActions = null;

		// Token: 0x04002532 RID: 9522
		public List<Action> preTickActions = null;

		// Token: 0x04002533 RID: 9523
		public List<Action> finishActions = null;

		// Token: 0x04002534 RID: 9524
		public bool atomicWithPrevious = false;

		// Token: 0x04002535 RID: 9525
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x04002536 RID: 9526
		public Func<SkillDef> activeSkill;

		// Token: 0x04002537 RID: 9527
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x04002538 RID: 9528
		public int defaultDuration = 0;

		// Token: 0x04002539 RID: 9529
		public bool handlingFacing = false;
	}
}

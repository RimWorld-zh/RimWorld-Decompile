using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A48 RID: 2632
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06003A83 RID: 14979 RVA: 0x001EFDFC File Offset: 0x001EE1FC
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

		// Token: 0x06003A84 RID: 14980 RVA: 0x001EFEE8 File Offset: 0x001EE2E8
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x001EFF04 File Offset: 0x001EE304
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

		// Token: 0x06003A86 RID: 14982 RVA: 0x001EFF36 File Offset: 0x001EE336
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001EFF45 File Offset: 0x001EE345
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001EFF6A File Offset: 0x001EE36A
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001EFF8F File Offset: 0x001EE38F
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x04002532 RID: 9522
		public Pawn actor;

		// Token: 0x04002533 RID: 9523
		public Action initAction = null;

		// Token: 0x04002534 RID: 9524
		public Action tickAction = null;

		// Token: 0x04002535 RID: 9525
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x04002536 RID: 9526
		public List<Action> preInitActions = null;

		// Token: 0x04002537 RID: 9527
		public List<Action> preTickActions = null;

		// Token: 0x04002538 RID: 9528
		public List<Action> finishActions = null;

		// Token: 0x04002539 RID: 9529
		public bool atomicWithPrevious = false;

		// Token: 0x0400253A RID: 9530
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x0400253B RID: 9531
		public Func<SkillDef> activeSkill;

		// Token: 0x0400253C RID: 9532
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x0400253D RID: 9533
		public int defaultDuration = 0;

		// Token: 0x0400253E RID: 9534
		public bool handlingFacing = false;
	}
}

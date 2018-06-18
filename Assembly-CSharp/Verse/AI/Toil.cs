using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A48 RID: 2632
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06003A85 RID: 14981 RVA: 0x001EFED0 File Offset: 0x001EE2D0
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

		// Token: 0x06003A86 RID: 14982 RVA: 0x001EFFBC File Offset: 0x001EE3BC
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001EFFD8 File Offset: 0x001EE3D8
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

		// Token: 0x06003A88 RID: 14984 RVA: 0x001F000A File Offset: 0x001EE40A
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001F0019 File Offset: 0x001EE419
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x001F003E File Offset: 0x001EE43E
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x001F0063 File Offset: 0x001EE463
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

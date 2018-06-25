using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A47 RID: 2631
	public sealed class Toil : IJobEndable
	{
		// Token: 0x0400253E RID: 9534
		public Pawn actor;

		// Token: 0x0400253F RID: 9535
		public Action initAction = null;

		// Token: 0x04002540 RID: 9536
		public Action tickAction = null;

		// Token: 0x04002541 RID: 9537
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x04002542 RID: 9538
		public List<Action> preInitActions = null;

		// Token: 0x04002543 RID: 9539
		public List<Action> preTickActions = null;

		// Token: 0x04002544 RID: 9540
		public List<Action> finishActions = null;

		// Token: 0x04002545 RID: 9541
		public bool atomicWithPrevious = false;

		// Token: 0x04002546 RID: 9542
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x04002547 RID: 9543
		public Func<SkillDef> activeSkill;

		// Token: 0x04002548 RID: 9544
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x04002549 RID: 9545
		public int defaultDuration = 0;

		// Token: 0x0400254A RID: 9546
		public bool handlingFacing = false;

		// Token: 0x06003A84 RID: 14980 RVA: 0x001F0568 File Offset: 0x001EE968
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

		// Token: 0x06003A85 RID: 14981 RVA: 0x001F0654 File Offset: 0x001EEA54
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x001F0670 File Offset: 0x001EEA70
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

		// Token: 0x06003A87 RID: 14983 RVA: 0x001F06A2 File Offset: 0x001EEAA2
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001F06B1 File Offset: 0x001EEAB1
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001F06D6 File Offset: 0x001EEAD6
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x001F06FB File Offset: 0x001EEAFB
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}
	}
}

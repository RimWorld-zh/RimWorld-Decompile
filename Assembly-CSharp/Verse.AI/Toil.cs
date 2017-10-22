using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public sealed class Toil : IJobEndable
	{
		public Pawn actor;

		public Action initAction = null;

		public Action tickAction = null;

		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		public List<Action> preInitActions = null;

		public List<Action> preTickActions = null;

		public List<Action> finishActions = null;

		public bool atomicWithPrevious = false;

		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		public int defaultDuration = 0;

		public bool handlingFacing = false;

		public void Cleanup()
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
						Log.Error("Pawn " + this.actor + " threw exception while executing toil's finish action (" + i + "), curJob=" + this.actor.CurJob + ": " + ex);
					}
				}
			}
		}

		public Pawn GetActor()
		{
			return this.actor;
		}

		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add((Func<JobCondition>)(() => (JobCondition)((!newFailCondition()) ? 1 : 3)));
		}

		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

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

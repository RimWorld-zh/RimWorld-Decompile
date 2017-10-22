using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_PlantWork : JobDriver
	{
		private float workDone = 0f;

		protected float xpPerTick = 0f;

		protected const TargetIndex PlantInd = TargetIndex.A;

		protected Plant Plant
		{
			get
			{
				return (Plant)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			LocalTargetInfo target = base.job.GetTarget(TargetIndex.A);
			bool result;
			if (target.IsValid && !base.pawn.Reserve(target, base.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				base.pawn.ReserveAsManyAsPossible(base.job.GetTargetQueue(TargetIndex.A), base.job, 1, -1, null);
				result = true;
			}
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004e: stateMachine*/;
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		protected virtual void Init()
		{
		}

		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}
	}
}

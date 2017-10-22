using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		private float workLeft;

		private float totalNeededWork;

		protected Thing Target
		{
			get
			{
				return base.CurJob.targetA.Thing;
			}
		}

		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		protected abstract DesignationDef Designation
		{
			get;
		}

		protected abstract int TotalNeededWork
		{
			get;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = (Action)delegate
			{
				((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.totalNeededWork = (float)((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.TotalNeededWork;
				((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.workLeft = ((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00a4: stateMachine*/)._003C_003Ef__this.totalNeededWork;
			};
			doWork.tickAction = (Action)delegate
			{
				((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00bb: stateMachine*/)._003C_003Ef__this.workLeft -= ((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00bb: stateMachine*/)._003C_003Ef__this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
				((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00bb: stateMachine*/)._003C_003Ef__this.TickAction();
				if (((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00bb: stateMachine*/)._003C_003Ef__this.workLeft <= 0.0)
				{
					((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00bb: stateMachine*/)._003CdoWork_003E__0.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, (Func<float>)(() => (float)(1.0 - ((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00df: stateMachine*/)._003C_003Ef__this.workLeft / ((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_00df: stateMachine*/)._003C_003Ef__this.totalNeededWork)), false, -0.5f);
			yield return doWork;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_0120: stateMachine*/)._003C_003Ef__this.FinishedRemoving();
					((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_0120: stateMachine*/)._003C_003Ef__this.Map.designationManager.RemoveAllDesignationsOn(((_003CMakeNewToils_003Ec__IteratorF)/*Error near IL_0120: stateMachine*/)._003C_003Ef__this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		protected virtual void FinishedRemoving()
		{
		}

		protected virtual void TickAction()
		{
		}
	}
}

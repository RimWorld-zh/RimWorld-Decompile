using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		private float workLeft = 0f;

		private float totalNeededWork = 0f;

		protected Thing Target
		{
			get
			{
				return base.job.targetA.Thing;
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

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Target, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected virtual void FinishedRemoving()
		{
		}

		protected virtual void TickAction()
		{
		}
	}
}

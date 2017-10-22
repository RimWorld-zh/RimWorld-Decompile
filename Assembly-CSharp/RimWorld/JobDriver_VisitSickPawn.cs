using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_VisitSickPawn : JobDriver
	{
		private const TargetIndex PatientInd = TargetIndex.A;

		private const TargetIndex ChairInd = TargetIndex.B;

		private Pawn Patient
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Chair
		{
			get
			{
				return base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return (byte)(base.pawn.Reserve((Thing)this.Patient, base.job, 1, -1, null) ? ((this.Chair == null || base.pawn.Reserve(this.Chair, base.job, 1, -1, null)) ? 1 : 0) : 0) != 0;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0041: stateMachine*/)._0024this.Patient.InBed() || !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0041: stateMachine*/)._0024this.Patient.Awake()));
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (this.Chair != null)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

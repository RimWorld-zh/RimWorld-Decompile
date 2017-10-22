using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		private float workLeft;

		private const TargetIndex CellInd = TargetIndex.A;

		private const TargetIndex GotoTargetInd = TargetIndex.B;

		private const float BaseWorkAmount = 65f;

		protected IntVec3 Cell
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		protected abstract PathEndMode PathEndMode
		{
			get;
		}

		protected abstract void DoEffect();

		protected abstract bool DoWorkFailOn();

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		public override bool TryMakePreToilReservations()
		{
			Pawn pawn = base.pawn;
			LocalTargetInfo target = this.Cell;
			Job job = base.job;
			ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
			return pawn.Reserve(target, job, 1, -1, ceiling);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

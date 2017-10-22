using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		private float workLeft = -1000f;

		protected bool clearSnow = false;

		protected abstract int BaseWorkAmount
		{
			get;
		}

		protected abstract DesignationDef DesDef
		{
			get;
		}

		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			Pawn pawn = base.pawn;
			LocalTargetInfo targetA = base.job.targetA;
			Job job = base.job;
			ReservationLayerDef floor = ReservationLayerDefOf.Floor;
			return pawn.Reserve(targetA, job, 1, -1, floor);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOn((Func<bool>)(() => (byte)((!_003CMakeNewToils_003Ec__Iterator._0024this.job.ignoreDesignations && _003CMakeNewToils_003Ec__Iterator._0024this.Map.designationManager.DesignationAt(_003CMakeNewToils_003Ec__Iterator._0024this.TargetLocA, _003CMakeNewToils_003Ec__Iterator._0024this.DesDef) == null) ? 1 : 0) != 0));
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected abstract void DoEffect(IntVec3 c);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}

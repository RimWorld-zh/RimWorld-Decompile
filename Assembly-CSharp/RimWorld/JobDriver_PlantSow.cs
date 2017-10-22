using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantSow : JobDriver
	{
		private float sowWorkDone = 0f;

		private Plant Plant
		{
			get
			{
				return (Plant)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003a: stateMachine*/;
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn((Func<bool>)(() => GenPlant.AdjacentSowBlocker(_003CMakeNewToils_003Ec__Iterator._0024this.job.plantDefToSow, _003CMakeNewToils_003Ec__Iterator._0024this.TargetA.Cell, _003CMakeNewToils_003Ec__Iterator._0024this.Map) != null)).FailOn((Func<bool>)(() => !_003CMakeNewToils_003Ec__Iterator._0024this.job.plantDefToSow.CanEverPlantAt(_003CMakeNewToils_003Ec__Iterator._0024this.TargetLocA, _003CMakeNewToils_003Ec__Iterator._0024this.Map)));
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		private const TargetIndex BarrelInd = TargetIndex.A;

		private const TargetIndex WortInd = TargetIndex.B;

		private const int Duration = 200;

		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Thing Wort
		{
			get
			{
				return base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Barrel, base.job, 1, -1, null) && base.pawn.Reserve(this.Wort, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			base.AddEndCondition(() => (JobCondition)((((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005d: stateMachine*/)._0024this.Barrel.SpaceLeftForWort > 0) ? 1 : 2));
			yield return Toils_General.DoAtomic(delegate
			{
				((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006f: stateMachine*/)._0024this.job.count = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006f: stateMachine*/)._0024this.Barrel.SpaceLeftForWort;
			});
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

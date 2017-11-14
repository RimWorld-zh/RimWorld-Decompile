using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Refuel : JobDriver
	{
		private const TargetIndex RefuelableInd = TargetIndex.A;

		private const TargetIndex FuelInd = TargetIndex.B;

		private const int RefuelingDuration = 240;

		protected Thing Refuelable
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		protected Thing Fuel
		{
			get
			{
				return base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Refuelable, base.job, 1, -1, null) && base.pawn.Reserve(this.Fuel, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate
			{
				ThingWithComps thingWithComps = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0050: stateMachine*/)._0024this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				if (thingWithComps != null)
				{
					CompFlickable comp = thingWithComps.GetComp<CompFlickable>();
					if (comp != null && !comp.SwitchIsOn)
					{
						return true;
					}
				}
				return false;
			});
			base.AddEndCondition(() => (JobCondition)((!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0068: stateMachine*/)._0024this.RefuelableComp.IsFull) ? 1 : 2));
			yield return Toils_General.DoAtomic(delegate
			{
				((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_007a: stateMachine*/)._0024this.job.count = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_007a: stateMachine*/)._0024this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

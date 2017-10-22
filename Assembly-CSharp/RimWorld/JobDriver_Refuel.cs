using System;
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
			this.FailOn((Func<bool>)delegate
			{
				ThingWithComps thingWithComps = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0051: stateMachine*/)._0024this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				bool result;
				if (thingWithComps != null)
				{
					CompFlickable comp = thingWithComps.GetComp<CompFlickable>();
					if (comp != null && !comp.SwitchIsOn)
					{
						result = true;
						goto IL_004e;
					}
				}
				result = false;
				goto IL_004e;
				IL_004e:
				return result;
			});
			base.AddEndCondition((Func<JobCondition>)(() => (JobCondition)((!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0069: stateMachine*/)._0024this.RefuelableComp.IsFull) ? 1 : 2)));
			yield return Toils_General.DoAtomic((Action)delegate
			{
				((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_007b: stateMachine*/)._0024this.job.count = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_007b: stateMachine*/)._0024this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

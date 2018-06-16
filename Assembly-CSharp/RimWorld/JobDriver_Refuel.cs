using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000079 RID: 121
	public class JobDriver_Refuel : JobDriver
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00023D80 File Offset: 0x00022180
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00023DAC File Offset: 0x000221AC
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00023DCC File Offset: 0x000221CC
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00023DF8 File Offset: 0x000221F8
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00023E54 File Offset: 0x00022254
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (!this.job.playerForced)
				{
					CompFlickable compFlickable = this.Refuelable.TryGetComp<CompFlickable>();
					if (compFlickable != null && !compFlickable.SwitchIsOn)
					{
						return true;
					}
				}
				return false;
			});
			base.AddEndCondition(() => (!this.RefuelableComp.IsFull) ? JobCondition.Ongoing : JobCondition.Succeeded);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil reserveFuel = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return reserveFuel;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveFuel, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
			yield break;
		}

		// Token: 0x04000228 RID: 552
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04000229 RID: 553
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x0400022A RID: 554
		private const int RefuelingDuration = 240;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200007A RID: 122
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x0400022C RID: 556
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x0400022D RID: 557
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x0400022E RID: 558
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x0400022F RID: 559
		private const int RefuelingDuration = 240;

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000343 RID: 835 RVA: 0x000241E0 File Offset: 0x000225E0
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0002420C File Offset: 0x0002260C
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0002422C File Offset: 0x0002262C
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00024258 File Offset: 0x00022658
		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000242AC File Offset: 0x000226AC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			base.AddEndCondition(() => (!this.RefuelableComp.IsFull) ? JobCondition.Ongoing : JobCondition.Succeeded);
			base.AddFailCondition(() => !this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			yield return Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.None);
			yield break;
		}
	}
}

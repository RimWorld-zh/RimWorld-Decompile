using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000068 RID: 104
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0001F2BC File Offset: 0x0001D6BC
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0001F2EC File Offset: 0x0001D6EC
		protected Thing Wort
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001F318 File Offset: 0x0001D718
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null) && this.pawn.Reserve(this.Wort, this.job, 1, -1, null);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001F374 File Offset: 0x0001D774
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			base.AddEndCondition(() => (this.Barrel.SpaceLeftForWort > 0) ? JobCondition.Ongoing : JobCondition.Succeeded);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.Barrel.SpaceLeftForWort;
			});
			Toil reserveWort = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return reserveWort;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveWort, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Barrel.AddWort(this.Wort);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04000208 RID: 520
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04000209 RID: 521
		private const TargetIndex WortInd = TargetIndex.B;

		// Token: 0x0400020A RID: 522
		private const int Duration = 200;
	}
}

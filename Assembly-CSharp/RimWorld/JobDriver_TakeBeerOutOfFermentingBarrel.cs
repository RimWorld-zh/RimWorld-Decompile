using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000081 RID: 129
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		// Token: 0x0400023A RID: 570
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x0400023B RID: 571
		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		// Token: 0x0400023C RID: 572
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x0400023D RID: 573
		private const int Duration = 200;

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000364 RID: 868 RVA: 0x000257F8 File Offset: 0x00023BF8
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00025828 File Offset: 0x00023C28
		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00025854 File Offset: 0x00023C54
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00025888 File Offset: 0x00023C88
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.Barrel.TakeOutBeer();
					GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null);
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					IntVec3 c;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
					{
						this.job.SetTarget(TargetIndex.C, c);
						this.job.SetTarget(TargetIndex.B, thing);
						this.job.count = thing.stackCount;
					}
					else
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
			yield break;
		}
	}
}

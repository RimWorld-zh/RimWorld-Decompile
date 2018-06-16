using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000084 RID: 132
	public class JobDriver_UnloadInventory : JobDriver
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00026908 File Offset: 0x00024D08
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00026938 File Offset: 0x00024D38
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0002696C File Offset: 0x00024D6C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(10);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn otherPawn = this.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
						IntVec3 c;
						if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
						{
							Thing thing;
							otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
							if (thing != null)
							{
								thing.SetForbidden(false, false);
							}
						}
						else
						{
							Thing thing2;
							otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
							this.job.count = thing2.stackCount;
							this.job.SetTarget(TargetIndex.B, thing2);
							this.job.SetTarget(TargetIndex.C, c);
							firstUnloadableThing.Thing.SetForbidden(false, false);
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
			yield break;
		}

		// Token: 0x04000240 RID: 576
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		// Token: 0x04000241 RID: 577
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		// Token: 0x04000242 RID: 578
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x04000243 RID: 579
		private const int UnloadDuration = 10;
	}
}

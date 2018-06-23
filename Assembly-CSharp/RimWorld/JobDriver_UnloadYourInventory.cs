using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000085 RID: 133
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		// Token: 0x04000244 RID: 580
		private int countToDrop = -1;

		// Token: 0x04000245 RID: 581
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		// Token: 0x04000246 RID: 582
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04000247 RID: 583
		private const int UnloadDuration = 10;

		// Token: 0x06000378 RID: 888 RVA: 0x00026CF3 File Offset: 0x000250F3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00026D10 File Offset: 0x00025110
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00026D28 File Offset: 0x00025128
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Wait(10);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.pawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
						IntVec3 c;
						if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
						{
							Thing thing;
							this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
							this.job.SetTarget(TargetIndex.B, c);
							this.countToDrop = firstUnloadableThing.Count;
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
					if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
						{
							this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
							this.job.count = this.countToDrop;
							this.job.SetTarget(TargetIndex.A, thing);
						}
						thing.SetForbidden(false, false);
					}
				}
			};
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			yield break;
		}
	}
}

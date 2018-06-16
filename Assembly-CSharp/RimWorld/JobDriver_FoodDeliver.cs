using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000095 RID: 149
	public class JobDriver_FoodDeliver : JobDriver
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0002A938 File Offset: 0x00028D38
		private Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0002A962 File Offset: 0x00028D62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0002A990 File Offset: 0x00028D90
		public override string GetReport()
		{
			string result;
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				result = this.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0002AA14 File Offset: 0x00028E14
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0002AA6C File Offset: 0x00028E6C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0002AAA0 File Offset: 0x00028EA0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (this.usingNutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			Toil toil2 = new Toil();
			toil2.initAction = delegate()
			{
				Pawn actor = toil2.actor;
				Job curJob = actor.jobs.curJob;
				actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil2.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			toil2.AddFailCondition(delegate
			{
				Pawn pawn = (Pawn)toil2.actor.jobs.curJob.targetB.Thing;
				return !pawn.IsPrisonerOfColony || !pawn.guest.CanBeBroughtFood;
			});
			yield return toil2;
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, null);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x04000254 RID: 596
		private bool usingNutrientPasteDispenser;

		// Token: 0x04000255 RID: 597
		private bool eatingFromInventory;

		// Token: 0x04000256 RID: 598
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x04000257 RID: 599
		private const TargetIndex DelivereeInd = TargetIndex.B;
	}
}

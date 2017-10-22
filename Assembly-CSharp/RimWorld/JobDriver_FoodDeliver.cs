using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FoodDeliver : JobDriver
	{
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		private bool usingNutrientPasteDispenser;

		private bool eatingFromInventory;

		private Pawn Deliveree
		{
			get
			{
				return (Pawn)base.CurJob.targetB.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		public override string GetReport()
		{
			if (base.CurJob.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser)
			{
				return base.CurJob.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", ((Pawn)(Thing)base.CurJob.targetB).LabelShort);
			}
			return base.GetReport();
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (base.pawn.inventory != null && base.pawn.inventory.Contains(base.TargetThingA));
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
			}
			else if (this.usingNutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, base.pawn);
			}
			else
			{
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			Toil toil = new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator4B)/*Error near IL_0159: stateMachine*/)._003Ctoil_003E__0.actor;
					Job curJob = actor.jobs.curJob;
					actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			toil.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			toil.AddFailCondition((Func<bool>)delegate
			{
				Pawn pawn = (Pawn)((_003CMakeNewToils_003Ec__Iterator4B)/*Error near IL_0189: stateMachine*/)._003Ctoil_003E__0.actor.jobs.curJob.targetB.Thing;
				if (!pawn.IsPrisonerOfColony)
				{
					return true;
				}
				if (!pawn.guest.CanBeBroughtFood)
				{
					return true;
				}
				return false;
			});
			yield return toil;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Thing thing = default(Thing);
					((_003CMakeNewToils_003Ec__Iterator4B)/*Error near IL_01c3: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.TryDropCarriedThing(((_003CMakeNewToils_003Ec__Iterator4B)/*Error near IL_01c3: stateMachine*/)._003Ctoil_003E__1.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, (Action<Thing, int>)null);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}

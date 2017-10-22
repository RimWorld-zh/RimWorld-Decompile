using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FoodDeliver : JobDriver
	{
		private bool usingNutrientPasteDispenser;

		private bool eatingFromInventory;

		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		private Pawn Deliveree
		{
			get
			{
				return (Pawn)base.job.targetB.Thing;
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
			return (!(base.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser) || this.Deliveree == null) ? base.GetReport() : base.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (base.pawn.inventory != null && base.pawn.inventory.Contains(base.TargetThingA));
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Deliveree, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.usingNutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

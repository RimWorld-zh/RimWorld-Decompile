using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		private const float FeedDurationMultiplier = 1.5f;

		protected Thing Food
		{
			get
			{
				return base.job.targetA.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)base.job.targetB.Thing;
			}
		}

		public override string GetReport()
		{
			if (base.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return base.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
			}
			return base.GetReport();
		}

		public override bool TryMakePreToilReservations()
		{
			if (!base.pawn.Reserve(this.Deliveree, base.job, 1, -1, null))
			{
				return false;
			}
			if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (base.pawn.inventory == null || !base.pawn.inventory.Contains(base.TargetThingA)) && !base.pawn.Reserve(this.Food, base.job, 1, -1, null))
			{
				return false;
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0050: stateMachine*/)._0024this.Deliveree));
			if (base.pawn.inventory != null && base.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

using System;
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
				return base.CurJob.targetA.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)base.CurJob.targetB.Thing;
			}
		}

		public override string GetReport()
		{
			if (base.CurJob.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser)
			{
				return base.CurJob.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", ((Pawn)(Thing)base.CurJob.targetB).LabelShort);
			}
			return base.GetReport();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn((Func<bool>)(() => !FoodUtility.ShouldBeFedBySomeone(((_003CMakeNewToils_003Ec__Iterator4C)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.Deliveree)));
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			if (base.pawn.inventory != null && base.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
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
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
		}
	}
}

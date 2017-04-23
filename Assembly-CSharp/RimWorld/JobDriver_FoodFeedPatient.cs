using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return base.CurJob.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", ((Pawn)((Thing)base.CurJob.targetB)).LabelShort);
			}
			return base.GetReport();
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FoodFeedPatient.<MakeNewToils>c__Iterator4C <MakeNewToils>c__Iterator4C = new JobDriver_FoodFeedPatient.<MakeNewToils>c__Iterator4C();
			<MakeNewToils>c__Iterator4C.<>f__this = this;
			JobDriver_FoodFeedPatient.<MakeNewToils>c__Iterator4C expr_0E = <MakeNewToils>c__Iterator4C;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}

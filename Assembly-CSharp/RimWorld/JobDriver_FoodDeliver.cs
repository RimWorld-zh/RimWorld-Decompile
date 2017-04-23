using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FoodDeliver : JobDriver
	{
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		private Pawn Deliveree
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
			JobDriver_FoodDeliver.<MakeNewToils>c__Iterator4B <MakeNewToils>c__Iterator4B = new JobDriver_FoodDeliver.<MakeNewToils>c__Iterator4B();
			<MakeNewToils>c__Iterator4B.<>f__this = this;
			JobDriver_FoodDeliver.<MakeNewToils>c__Iterator4B expr_0E = <MakeNewToils>c__Iterator4B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}

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
				return base.CurJob.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", ((Pawn)((Thing)base.CurJob.targetB)).LabelShort);
			}
			return base.GetReport();
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
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

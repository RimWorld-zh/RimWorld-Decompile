using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class BillRepeatModeUtility
	{
		public static void MakeConfigFloatMenu(Bill_Production bill)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.RepeatCount.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			FloatMenuOption item = new FloatMenuOption(BillRepeatModeDefOf.TargetCount.LabelCap, delegate()
			{
				if (!bill.recipe.WorkerCounter.CanCountProducts(bill))
				{
					Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					bill.repeatMode = BillRepeatModeDefOf.TargetCount;
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			list.Add(item);
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.Forever.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.Forever;
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[CompilerGenerated]
		private sealed class <MakeConfigFloatMenu>c__AnonStorey0
		{
			internal Bill_Production bill;

			public <MakeConfigFloatMenu>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}

			internal void <>m__1()
			{
				if (!this.bill.recipe.WorkerCounter.CanCountProducts(this.bill))
				{
					Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					this.bill.repeatMode = BillRepeatModeDefOf.TargetCount;
				}
			}

			internal void <>m__2()
			{
				this.bill.repeatMode = BillRepeatModeDefOf.Forever;
			}
		}
	}
}

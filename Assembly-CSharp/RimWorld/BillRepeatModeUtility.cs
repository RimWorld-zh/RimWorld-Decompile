using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B3 RID: 1715
	public static class BillRepeatModeUtility
	{
		// Token: 0x060024E2 RID: 9442 RVA: 0x0013BBFC File Offset: 0x00139FFC
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
	}
}

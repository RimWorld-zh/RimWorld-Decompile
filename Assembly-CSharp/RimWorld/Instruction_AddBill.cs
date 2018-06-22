using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B4 RID: 2228
	public class Instruction_AddBill : Lesson_Instruction
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x001B7830 File Offset: 0x001B5C30
		protected override float ProgressPercent
		{
			get
			{
				int num = this.def.recipeTargetCount + 1;
				int num2 = 0;
				Bill_Production bill_Production = this.RelevantBill();
				if (bill_Production != null)
				{
					num2++;
					if (bill_Production.repeatMode == BillRepeatModeDefOf.RepeatCount)
					{
						num2 += bill_Production.repeatCount;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x001B7888 File Offset: 0x001B5C88
		private Bill_Production RelevantBill()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == this.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Bill b) => b.recipe == this.def.recipeDef);
				}
			}
			return null;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x001B790C File Offset: 0x001B5D0C
		private IEnumerable<Thing> ThingsToSelect()
		{
			if (Find.Selector.SingleSelectedThing == null || Find.Selector.SingleSelectedThing.def != this.def.thingDef)
			{
				foreach (Building billGiver in base.Map.listerBuildings.AllBuildingsColonistOfDef(this.def.thingDef))
				{
					yield return billGiver;
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x001B7938 File Offset: 0x001B5D38
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.ThingsToSelect())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			if (this.RelevantBill() == null)
			{
				UIHighlighter.HighlightTag("AddBill");
			}
			base.LessonOnGUI();
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x001B79BC File Offset: 0x001B5DBC
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.ThingsToSelect())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
			}
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

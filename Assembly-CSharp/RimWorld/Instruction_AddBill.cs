using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B6 RID: 2230
	public class Instruction_AddBill : Lesson_Instruction
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x001B7C44 File Offset: 0x001B6044
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

		// Token: 0x06003303 RID: 13059 RVA: 0x001B7C9C File Offset: 0x001B609C
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

		// Token: 0x06003304 RID: 13060 RVA: 0x001B7D20 File Offset: 0x001B6120
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

		// Token: 0x06003305 RID: 13061 RVA: 0x001B7D4C File Offset: 0x001B614C
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

		// Token: 0x06003306 RID: 13062 RVA: 0x001B7DD0 File Offset: 0x001B61D0
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

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_AddBill : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				int num = base.def.recipeTargetCount + 1;
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

		private Bill_Production RelevantBill()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == base.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Func<Bill, bool>)((Bill b) => b.recipe == base.def.recipeDef));
				}
			}
			return null;
		}

		private IEnumerable<Thing> ThingsToSelect()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == base.def.thingDef)
				yield break;
			foreach (Building item in base.Map.listerBuildings.AllBuildingsColonistOfDef(base.def.thingDef))
			{
				yield return (Thing)item;
			}
		}

		public override void LessonOnGUI()
		{
			foreach (Thing item in this.ThingsToSelect())
			{
				TutorUtility.DrawLabelOnThingOnGUI(item, base.def.onMapInstruction);
			}
			if (this.RelevantBill() == null)
			{
				UIHighlighter.HighlightTag("AddBill");
			}
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			foreach (Thing item in this.ThingsToSelect())
			{
				GenDraw.DrawArrowPointingAt(item.DrawPos, false);
			}
			if (this.ProgressPercent > 0.99900001287460327)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

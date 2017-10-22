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
			Bill_Production result;
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == base.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					result = (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Func<Bill, bool>)((Bill b) => b.recipe == base.def.recipeDef));
					goto IL_0076;
				}
			}
			result = null;
			goto IL_0076;
			IL_0076:
			return result;
		}

		private IEnumerable<Thing> ThingsToSelect()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == base.def.thingDef)
				yield break;
			using (IEnumerator<Building> enumerator = base.Map.listerBuildings.AllBuildingsColonistOfDef(base.def.thingDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Building billGiver = enumerator.Current;
					yield return (Thing)billGiver;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0116:
			/*Error near IL_0117: Unexpected return in MoveNext()*/;
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

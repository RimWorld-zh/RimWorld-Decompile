using System;
using Verse;

namespace RimWorld
{
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		protected Instruction_BuildAtRoom()
		{
		}

		protected abstract CellRect BuildableRect { get; }

		protected override float ProgressPercent
		{
			get
			{
				float result;
				if (this.def.targetCount <= 1)
				{
					result = -1f;
				}
				else
				{
					result = (float)this.NumPlaced() / (float)this.def.targetCount;
				}
				return result;
			}
		}

		protected int NumPlaced()
		{
			int num = 0;
			foreach (IntVec3 c in this.BuildableRect)
			{
				if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, this.def.thingDef))
				{
					num++;
				}
			}
			return num;
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				result = this.AllowBuildAt(ep.Cell);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

using Verse;

namespace RimWorld
{
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		protected abstract CellRect BuildableRect
		{
			get;
		}

		protected override float ProgressPercent
		{
			get
			{
				if (base.def.targetCount <= 1)
				{
					return -1f;
				}
				return (float)this.NumPlaced() / (float)base.def.targetCount;
			}
		}

		protected int NumPlaced()
		{
			int num = 0;
			foreach (IntVec3 item in this.BuildableRect)
			{
				if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(item, base.Map, base.def.thingDef))
				{
					num++;
				}
			}
			return num;
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), base.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + base.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= base.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

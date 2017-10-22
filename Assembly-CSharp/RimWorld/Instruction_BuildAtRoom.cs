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
				return (float)((base.def.targetCount > 1) ? ((float)this.NumPlaced() / (float)base.def.targetCount) : -1.0);
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
			return (!(ep.Tag == "Designate-" + base.def.thingDef.defName)) ? base.AllowAction(ep) : this.AllowBuildAt(ep.Cell);
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

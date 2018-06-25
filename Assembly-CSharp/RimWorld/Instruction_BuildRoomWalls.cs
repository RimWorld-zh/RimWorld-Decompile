using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();

		public Instruction_BuildRoomWalls()
		{
		}

		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.RoomRect.EdgeCells)
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall))
					{
						num2++;
					}
					num++;
				}
				return (float)num2 / (float)num;
			}
		}

		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-Wall")
			{
				result = TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		[CompilerGenerated]
		private bool <LessonUpdate>m__0(IntVec3 c)
		{
			return !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall);
		}

		[CompilerGenerated]
		private bool <LessonUpdate>m__1(IntVec3 c)
		{
			return c.GetEdifice(base.Map) == null;
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		private List<IntVec3> allowedPlaceCells;

		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList();
			this.allowedPlaceCells.RemoveAll(delegate(IntVec3 c)
			{
				int x = c.x;
				CellRect roomRect = this.RoomRect;
				if (x == roomRect.minX)
				{
					int z = c.z;
					CellRect roomRect2 = this.RoomRect;
					if (z != roomRect2.minZ)
						goto IL_0034;
					goto IL_00d6;
				}
				goto IL_0034;
				IL_00d6:
				int result = 1;
				goto IL_00d7;
				IL_0034:
				int x2 = c.x;
				CellRect roomRect3 = this.RoomRect;
				if (x2 == roomRect3.minX)
				{
					int z2 = c.z;
					CellRect roomRect4 = this.RoomRect;
					if (z2 != roomRect4.maxZ)
						goto IL_0068;
					goto IL_00d6;
				}
				goto IL_0068;
				IL_009e:
				int x3 = c.x;
				CellRect roomRect5 = this.RoomRect;
				if (x3 == roomRect5.maxX)
				{
					int z3 = c.z;
					CellRect roomRect6 = this.RoomRect;
					result = ((z3 == roomRect6.maxZ) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				goto IL_00d7;
				IL_0068:
				int x4 = c.x;
				CellRect roomRect7 = this.RoomRect;
				if (x4 == roomRect7.maxX)
				{
					int z4 = c.z;
					CellRect roomRect8 = this.RoomRect;
					if (z4 != roomRect8.minZ)
						goto IL_009e;
					goto IL_00d6;
				}
				goto IL_009e;
				IL_00d7:
				return (byte)result != 0;
			});
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, base.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

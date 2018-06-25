using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		private List<IntVec3> allowedPlaceCells;

		public Instruction_BuildRoomDoor()
		{
		}

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
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-Door")
			{
				result = TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		[CompilerGenerated]
		private bool <OnActivated>m__0(IntVec3 c)
		{
			return (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ);
		}
	}
}

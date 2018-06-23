using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B9 RID: 2233
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x04001B8D RID: 7053
		private List<IntVec3> allowedPlaceCells;

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x001B7F14 File Offset: 0x001B6314
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x001B7F34 File Offset: 0x001B6334
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x001B7F79 File Offset: 0x001B6379
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x001B7F98 File Offset: 0x001B6398
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x001B7FBC File Offset: 0x001B63BC
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

		// Token: 0x06003319 RID: 13081 RVA: 0x001B8005 File Offset: 0x001B6405
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

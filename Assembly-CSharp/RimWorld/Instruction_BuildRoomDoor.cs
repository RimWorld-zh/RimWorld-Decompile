using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BB RID: 2235
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x04001B8D RID: 7053
		private List<IntVec3> allowedPlaceCells;

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x001B8054 File Offset: 0x001B6454
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x001B8074 File Offset: 0x001B6474
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x001B80B9 File Offset: 0x001B64B9
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x001B80D8 File Offset: 0x001B64D8
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x001B80FC File Offset: 0x001B64FC
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

		// Token: 0x0600331D RID: 13085 RVA: 0x001B8145 File Offset: 0x001B6545
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

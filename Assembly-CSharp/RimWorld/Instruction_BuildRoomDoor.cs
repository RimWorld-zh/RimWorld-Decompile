using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BB RID: 2235
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x04001B93 RID: 7059
		private List<IntVec3> allowedPlaceCells;

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x001B8328 File Offset: 0x001B6728
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x001B8348 File Offset: 0x001B6748
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x001B838D File Offset: 0x001B678D
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x001B83AC File Offset: 0x001B67AC
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x001B83D0 File Offset: 0x001B67D0
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

		// Token: 0x0600331D RID: 13085 RVA: 0x001B8419 File Offset: 0x001B6819
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}

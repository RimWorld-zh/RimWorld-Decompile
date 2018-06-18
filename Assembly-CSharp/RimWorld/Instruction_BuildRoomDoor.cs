using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BD RID: 2237
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x001B7D2C File Offset: 0x001B612C
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x001B7D4C File Offset: 0x001B614C
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x001B7D91 File Offset: 0x001B6191
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x001B7DB0 File Offset: 0x001B61B0
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x001B7DD4 File Offset: 0x001B61D4
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

		// Token: 0x06003320 RID: 13088 RVA: 0x001B7E1D File Offset: 0x001B621D
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04001B8F RID: 7055
		private List<IntVec3> allowedPlaceCells;
	}
}

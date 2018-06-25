using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x04001BA0 RID: 7072
		private CellRect growingZoneRect;

		// Token: 0x04001BA1 RID: 7073
		private List<IntVec3> cachedCells;

		// Token: 0x06003361 RID: 13153 RVA: 0x001B96C0 File Offset: 0x001B7AC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x001B96FF File Offset: 0x001B7AFF
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x001B9718 File Offset: 0x001B7B18
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x001B9741 File Offset: 0x001B7B41
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x001B9760 File Offset: 0x001B7B60
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x001B9780 File Offset: 0x001B7B80
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				result = TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C9 RID: 2249
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x06003364 RID: 13156 RVA: 0x001B90C4 File Offset: 0x001B74C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x001B9103 File Offset: 0x001B7503
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x001B911C File Offset: 0x001B751C
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x001B9145 File Offset: 0x001B7545
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x001B9164 File Offset: 0x001B7564
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001B9184 File Offset: 0x001B7584
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

		// Token: 0x04001B9C RID: 7068
		private CellRect growingZoneRect;

		// Token: 0x04001B9D RID: 7069
		private List<IntVec3> cachedCells;
	}
}

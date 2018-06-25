using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x04001B9A RID: 7066
		private CellRect growingZoneRect;

		// Token: 0x04001B9B RID: 7067
		private List<IntVec3> cachedCells;

		// Token: 0x06003361 RID: 13153 RVA: 0x001B93EC File Offset: 0x001B77EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x001B942B File Offset: 0x001B782B
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x001B9444 File Offset: 0x001B7844
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x001B946D File Offset: 0x001B786D
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x001B948C File Offset: 0x001B788C
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x001B94AC File Offset: 0x001B78AC
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

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CA RID: 2250
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x0600336B RID: 13163 RVA: 0x001B91D8 File Offset: 0x001B75D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001B9217 File Offset: 0x001B7617
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001B9230 File Offset: 0x001B7630
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001B9258 File Offset: 0x001B7658
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x001B9277 File Offset: 0x001B7677
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x001B9298 File Offset: 0x001B7698
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				result = TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		// Token: 0x04001B9E RID: 7070
		private CellRect stockpileRect;

		// Token: 0x04001B9F RID: 7071
		private List<IntVec3> cachedCells;
	}
}

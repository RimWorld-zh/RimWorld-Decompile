using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x04001B9C RID: 7068
		private CellRect stockpileRect;

		// Token: 0x04001B9D RID: 7069
		private List<IntVec3> cachedCells;

		// Token: 0x06003368 RID: 13160 RVA: 0x001B9500 File Offset: 0x001B7900
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001B953F File Offset: 0x001B793F
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x001B9558 File Offset: 0x001B7958
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x001B9580 File Offset: 0x001B7980
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001B959F File Offset: 0x001B799F
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001B95C0 File Offset: 0x001B79C0
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
	}
}

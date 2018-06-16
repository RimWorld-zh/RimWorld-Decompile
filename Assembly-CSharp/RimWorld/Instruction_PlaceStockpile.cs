using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CA RID: 2250
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06003369 RID: 13161 RVA: 0x001B9110 File Offset: 0x001B7510
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x001B914F File Offset: 0x001B754F
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x001B9168 File Offset: 0x001B7568
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001B9190 File Offset: 0x001B7590
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001B91AF File Offset: 0x001B75AF
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001B91D0 File Offset: 0x001B75D0
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

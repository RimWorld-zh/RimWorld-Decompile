using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x04001BA2 RID: 7074
		private CellRect stockpileRect;

		// Token: 0x04001BA3 RID: 7075
		private List<IntVec3> cachedCells;

		// Token: 0x06003368 RID: 13160 RVA: 0x001B97D4 File Offset: 0x001B7BD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001B9813 File Offset: 0x001B7C13
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x001B982C File Offset: 0x001B7C2C
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x001B9854 File Offset: 0x001B7C54
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001B9873 File Offset: 0x001B7C73
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001B9894 File Offset: 0x001B7C94
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

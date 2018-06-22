using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C6 RID: 2246
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06003364 RID: 13156 RVA: 0x001B93C0 File Offset: 0x001B77C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x001B93FF File Offset: 0x001B77FF
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x001B9418 File Offset: 0x001B7818
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x001B9440 File Offset: 0x001B7840
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x001B945F File Offset: 0x001B785F
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001B9480 File Offset: 0x001B7880
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

		// Token: 0x04001B9C RID: 7068
		private CellRect stockpileRect;

		// Token: 0x04001B9D RID: 7069
		private List<IntVec3> cachedCells;
	}
}

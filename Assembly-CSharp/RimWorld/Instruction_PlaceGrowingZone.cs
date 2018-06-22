using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x001B92AC File Offset: 0x001B76AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x001B92EB File Offset: 0x001B76EB
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x001B9304 File Offset: 0x001B7704
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x001B932D File Offset: 0x001B772D
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x001B934C File Offset: 0x001B774C
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x001B936C File Offset: 0x001B776C
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

		// Token: 0x04001B9A RID: 7066
		private CellRect growingZoneRect;

		// Token: 0x04001B9B RID: 7067
		private List<IntVec3> cachedCells;
	}
}

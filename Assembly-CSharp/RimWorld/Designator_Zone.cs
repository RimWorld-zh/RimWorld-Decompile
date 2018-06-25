using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E6 RID: 2022
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x0017A558 File Offset: 0x00178958
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0017A570 File Offset: 0x00178970
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x0017A586 File Offset: 0x00178986
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			OverlayDrawHandler.DrawZonesThisFrame();
			if (Find.Selector.SelectedZone != null)
			{
				GenDraw.DrawFieldEdges(Find.Selector.SelectedZone.Cells);
			}
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x0017A5C1 File Offset: 0x001789C1
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

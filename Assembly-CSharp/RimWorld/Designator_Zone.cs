using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0017A19C File Offset: 0x0017859C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x0017A1B4 File Offset: 0x001785B4
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x0017A1CA File Offset: 0x001785CA
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

		// Token: 0x06002CF7 RID: 11511 RVA: 0x0017A205 File Offset: 0x00178605
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

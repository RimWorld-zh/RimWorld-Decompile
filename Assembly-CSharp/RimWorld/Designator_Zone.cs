using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x0017A230 File Offset: 0x00178630
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x0017A248 File Offset: 0x00178648
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x0017A25E File Offset: 0x0017865E
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

		// Token: 0x06002CF9 RID: 11513 RVA: 0x0017A299 File Offset: 0x00178699
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E4 RID: 2020
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002CEF RID: 11503 RVA: 0x0017A408 File Offset: 0x00178808
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x0017A420 File Offset: 0x00178820
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x0017A436 File Offset: 0x00178836
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

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0017A471 File Offset: 0x00178871
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

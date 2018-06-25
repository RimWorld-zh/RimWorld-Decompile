using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E6 RID: 2022
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x0017A7BC File Offset: 0x00178BBC
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x0017A7D4 File Offset: 0x00178BD4
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x0017A7EA File Offset: 0x00178BEA
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

		// Token: 0x06002CF5 RID: 11509 RVA: 0x0017A825 File Offset: 0x00178C25
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

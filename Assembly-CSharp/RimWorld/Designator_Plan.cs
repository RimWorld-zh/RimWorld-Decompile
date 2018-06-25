using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D3 RID: 2003
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x040017A2 RID: 6050
		private DesignateMode mode;

		// Token: 0x06002C6E RID: 11374 RVA: 0x00176BD0 File Offset: 0x00174FD0
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x00176C08 File Offset: 0x00175008
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x00176C20 File Offset: 0x00175020
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x00176C38 File Offset: 0x00175038
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x00176C54 File Offset: 0x00175054
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.InNoBuildEdgeArea(base.Map))
			{
				result = "TooCloseToMapEdge".Translate();
			}
			else
			{
				if (this.mode == DesignateMode.Add)
				{
					if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
					{
						return false;
					}
				}
				else if (this.mode == DesignateMode.Remove)
				{
					if (base.Map.designationManager.DesignationAt(c, this.Designation) == null)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x00176D24 File Offset: 0x00175124
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, this.Designation));
			}
			else if (this.mode == DesignateMode.Remove)
			{
				base.Map.designationManager.DesignationAt(c, this.Designation).Delete();
			}
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x00176D8B File Offset: 0x0017518B
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x00176D98 File Offset: 0x00175198
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

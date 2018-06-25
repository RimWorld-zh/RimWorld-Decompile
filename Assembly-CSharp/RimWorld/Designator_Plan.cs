using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D3 RID: 2003
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x040017A6 RID: 6054
		private DesignateMode mode;

		// Token: 0x06002C6D RID: 11373 RVA: 0x00176E34 File Offset: 0x00175234
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x00176E6C File Offset: 0x0017526C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x00176E84 File Offset: 0x00175284
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x00176E9C File Offset: 0x0017529C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00176EB8 File Offset: 0x001752B8
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

		// Token: 0x06002C72 RID: 11378 RVA: 0x00176F88 File Offset: 0x00175388
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

		// Token: 0x06002C73 RID: 11379 RVA: 0x00176FEF File Offset: 0x001753EF
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x00176FFC File Offset: 0x001753FC
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

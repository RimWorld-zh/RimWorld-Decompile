using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x06002C6F RID: 11375 RVA: 0x00176814 File Offset: 0x00174C14
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x0017684C File Offset: 0x00174C4C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x00176864 File Offset: 0x00174C64
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x0017687C File Offset: 0x00174C7C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x00176898 File Offset: 0x00174C98
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

		// Token: 0x06002C74 RID: 11380 RVA: 0x00176968 File Offset: 0x00174D68
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

		// Token: 0x06002C75 RID: 11381 RVA: 0x001769CF File Offset: 0x00174DCF
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x001769DC File Offset: 0x00174DDC
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x040017A4 RID: 6052
		private DesignateMode mode;
	}
}

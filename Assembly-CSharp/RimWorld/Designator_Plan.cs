using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x06002C71 RID: 11377 RVA: 0x001768A8 File Offset: 0x00174CA8
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x001768E0 File Offset: 0x00174CE0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x001768F8 File Offset: 0x00174CF8
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x00176910 File Offset: 0x00174D10
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0017692C File Offset: 0x00174D2C
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

		// Token: 0x06002C76 RID: 11382 RVA: 0x001769FC File Offset: 0x00174DFC
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

		// Token: 0x06002C77 RID: 11383 RVA: 0x00176A63 File Offset: 0x00174E63
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x00176A70 File Offset: 0x00174E70
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x040017A4 RID: 6052
		private DesignateMode mode;
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D1 RID: 2001
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x040017A2 RID: 6050
		private DesignateMode mode;

		// Token: 0x06002C6A RID: 11370 RVA: 0x00176A80 File Offset: 0x00174E80
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00176AB8 File Offset: 0x00174EB8
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x00176AD0 File Offset: 0x00174ED0
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x00176AE8 File Offset: 0x00174EE8
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x00176B04 File Offset: 0x00174F04
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

		// Token: 0x06002C6F RID: 11375 RVA: 0x00176BD4 File Offset: 0x00174FD4
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

		// Token: 0x06002C70 RID: 11376 RVA: 0x00176C3B File Offset: 0x0017503B
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00176C48 File Offset: 0x00175048
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class Designator_SmoothSurface : Designator
	{
		// Token: 0x06002C90 RID: 11408 RVA: 0x00177898 File Offset: 0x00175C98
		public Designator_SmoothSurface()
		{
			this.defaultLabel = "DesignatorSmoothSurface".Translate();
			this.defaultDesc = "DesignatorSmoothSurfaceDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SmoothSurface", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_SmoothSurface;
			this.hotKey = KeyBindingDefOf.Misc1;
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x00177910 File Offset: 0x00175D10
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x00177928 File Offset: 0x00175D28
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x00177940 File Offset: 0x00175D40
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.Fogged(base.Map))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor) != null || base.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) != null)
			{
				result = "SurfaceBeingSmoothed".Translate();
			}
			else if (c.InNoBuildEdgeArea(base.Map))
			{
				result = "TooCloseToMapEdge".Translate();
			}
			else
			{
				Building edifice = c.GetEdifice(base.Map);
				if (edifice != null && edifice.def.IsSmoothable)
				{
					result = AcceptanceReport.WasAccepted;
				}
				else if (edifice != null && !SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(edifice))
				{
					result = "MessageMustDesignateSmoothableSurface".Translate();
				}
				else
				{
					TerrainDef terrain = c.GetTerrain(base.Map);
					if (!terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone))
					{
						result = "MessageMustDesignateSmoothableSurface".Translate();
					}
					else
					{
						result = AcceptanceReport.WasAccepted;
					}
				}
			}
			return result;
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x00177A90 File Offset: 0x00175E90
		public override void DesignateSingleCell(IntVec3 c)
		{
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothWall));
				base.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothFloor));
			}
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x00177B1D File Offset: 0x00175F1D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x00177B25 File Offset: 0x00175F25
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

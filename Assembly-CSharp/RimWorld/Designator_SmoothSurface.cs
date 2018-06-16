using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DD RID: 2013
	public class Designator_SmoothSurface : Designator
	{
		// Token: 0x06002C95 RID: 11413 RVA: 0x0017762C File Offset: 0x00175A2C
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

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x001776A4 File Offset: 0x00175AA4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x001776BC File Offset: 0x00175ABC
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x001776D4 File Offset: 0x00175AD4
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

		// Token: 0x06002C99 RID: 11417 RVA: 0x00177824 File Offset: 0x00175C24
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

		// Token: 0x06002C9A RID: 11418 RVA: 0x001778B1 File Offset: 0x00175CB1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x001778B9 File Offset: 0x00175CB9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

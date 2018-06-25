using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DB RID: 2011
	public class Designator_SmoothSurface : Designator
	{
		// Token: 0x06002C94 RID: 11412 RVA: 0x001779E8 File Offset: 0x00175DE8
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
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x00177A60 File Offset: 0x00175E60
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x00177A78 File Offset: 0x00175E78
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x00177A90 File Offset: 0x00175E90
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

		// Token: 0x06002C98 RID: 11416 RVA: 0x00177BE0 File Offset: 0x00175FE0
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

		// Token: 0x06002C99 RID: 11417 RVA: 0x00177C6D File Offset: 0x0017606D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00177C75 File Offset: 0x00176075
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

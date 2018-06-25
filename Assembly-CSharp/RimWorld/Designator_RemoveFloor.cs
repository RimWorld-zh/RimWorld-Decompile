using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D8 RID: 2008
	public class Designator_RemoveFloor : Designator
	{
		// Token: 0x06002C81 RID: 11393 RVA: 0x00177494 File Offset: 0x00175894
		public Designator_RemoveFloor()
		{
			this.defaultLabel = "DesignatorRemoveFloor".Translate();
			this.defaultDesc = "DesignatorRemoveFloorDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveFloor", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_SmoothSurface;
			this.hotKey = KeyBindingDefOf.Misc1;
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x0017750C File Offset: 0x0017590C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x00177524 File Offset: 0x00175924
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0017753C File Offset: 0x0017593C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				result = false;
			}
			else if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(base.Map);
				if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
				{
					result = false;
				}
				else if (!base.Map.terrainGrid.CanRemoveTopLayerAt(c))
				{
					result = "TerrainMustBeRemovable".Translate();
				}
				else if (WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, base.Map))
				{
					result = false;
				}
				else
				{
					result = AcceptanceReport.WasAccepted;
				}
			}
			return result;
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x00177634 File Offset: 0x00175A34
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.RemoveTopLayer(c, true);
			}
			else
			{
				base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemoveFloor));
			}
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x00177684 File Offset: 0x00175A84
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x0017768C File Offset: 0x00175A8C
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

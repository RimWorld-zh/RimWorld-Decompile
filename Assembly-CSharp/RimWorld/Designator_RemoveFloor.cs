using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public class Designator_RemoveFloor : Designator
	{
		// Token: 0x06002C7E RID: 11390 RVA: 0x001770E0 File Offset: 0x001754E0
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
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x00177158 File Offset: 0x00175558
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002C80 RID: 11392 RVA: 0x00177170 File Offset: 0x00175570
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00177188 File Offset: 0x00175588
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

		// Token: 0x06002C82 RID: 11394 RVA: 0x00177280 File Offset: 0x00175680
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

		// Token: 0x06002C83 RID: 11395 RVA: 0x001772D0 File Offset: 0x001756D0
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x001772D8 File Offset: 0x001756D8
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}

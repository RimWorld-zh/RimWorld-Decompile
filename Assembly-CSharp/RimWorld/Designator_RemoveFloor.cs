using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_RemoveFloor : Designator
	{
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		public Designator_RemoveFloor()
		{
			base.defaultLabel = "DesignatorRemoveFloor".Translate();
			base.defaultDesc = "DesignatorRemoveFloorDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveFloor", true);
			base.useMouseIcon = true;
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.soundSucceeded = SoundDefOf.DesignateSmoothFloor;
			base.hotKey = KeyBindingDefOf.Misc1;
		}

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
				result = ((edifice == null || edifice.def.Fillage != FillCategory.Full || edifice.def.passability != Traversability.Impassable) ? (base.Map.terrainGrid.CanRemoveTopLayerAt(c) ? AcceptanceReport.WasAccepted : "TerrainMustBeRemovable".Translate()) : false);
			}
			return result;
		}

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

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}

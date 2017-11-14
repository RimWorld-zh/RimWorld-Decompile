using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaIgnoreRoof : Designator
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

		public Designator_AreaIgnoreRoof()
		{
			base.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			base.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			base.hotKey = KeyBindingDefOf.Misc11;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
			base.useMouseIcon = true;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			return ((Area)base.Map.areaManager.BuildRoof)[c] || ((Area)base.Map.areaManager.NoRoof)[c];
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			((Area)base.Map.areaManager.BuildRoof)[c] = false;
			((Area)base.Map.areaManager.NoRoof)[c] = false;
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}

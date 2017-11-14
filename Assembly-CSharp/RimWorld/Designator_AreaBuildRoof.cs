using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaBuildRoof : Designator
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

		public Designator_AreaBuildRoof()
		{
			base.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			base.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			base.hotKey = KeyBindingDefOf.Misc10;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
			base.useMouseIcon = true;
			base.tutorTag = "AreaBuildRoofExpand";
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
			bool flag = ((Area)base.Map.areaManager.BuildRoof)[c];
			return !flag;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			((Area)base.Map.areaManager.BuildRoof)[c] = true;
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

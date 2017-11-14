using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaNoRoof : Designator
	{
		private static List<IntVec3> justAddedCells = new List<IntVec3>();

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

		public Designator_AreaNoRoof()
		{
			base.defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
			base.defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea", true);
			base.hotKey = KeyBindingDefOf.Misc5;
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
			RoofDef roofDef = base.Map.roofGrid.RoofAt(c);
			if (roofDef != null && roofDef.isThickRoof)
			{
				return "MessageNothingCanRemoveThickRoofs".Translate();
			}
			bool flag = ((Area)base.Map.areaManager.NoRoof)[c];
			return !flag;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			((Area)base.Map.areaManager.NoRoof)[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				((Area)base.Map.areaManager.BuildRoof)[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}

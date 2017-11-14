using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		public Designator_AreaAllowedClear()
			: base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			base.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaDelete;
			base.hotKey = KeyBindingDefOf.Misc9;
			base.tutorTag = "AreaAllowedClear";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}

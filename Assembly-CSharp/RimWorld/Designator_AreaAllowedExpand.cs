using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		public Designator_AreaAllowedExpand()
			: base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			base.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
			base.hotKey = KeyBindingDefOf.Misc8;
			base.tutorTag = "AreaAllowedExpand";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}

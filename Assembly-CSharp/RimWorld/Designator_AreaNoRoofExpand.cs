using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaNoRoofExpand : Designator_AreaNoRoof
	{
		public Designator_AreaNoRoofExpand() : base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
			base.defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofAreaOn", true);
			base.hotKey = KeyBindingDefOf.Misc5;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
		}
	}
}

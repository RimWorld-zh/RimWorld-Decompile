using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaBuildRoofExpand : Designator_AreaBuildRoof
	{
		public Designator_AreaBuildRoofExpand() : base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			base.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofAreaExpand", true);
			base.hotKey = KeyBindingDefOf.Misc10;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
			base.tutorTag = "AreaBuildRoofExpand";
		}
	}
}

using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			base.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
			base.tutorTag = "AreaHomeExpand";
		}
	}
}

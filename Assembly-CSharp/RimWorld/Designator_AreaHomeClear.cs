using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		public Designator_AreaHomeClear()
			: base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorAreaHomeClear".Translate();
			base.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaDelete;
		}
	}
}

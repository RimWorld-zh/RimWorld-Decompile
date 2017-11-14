using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		public Designator_AreaSnowClearExpand()
			: base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorAreaSnowClearExpand".Translate();
			base.defaultDesc = "DesignatorAreaSnowClearExpandDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOn", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaAdd;
		}
	}
}

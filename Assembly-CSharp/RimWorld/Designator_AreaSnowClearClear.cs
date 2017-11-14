using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		public Designator_AreaSnowClearClear()
			: base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorAreaSnowClearClear".Translate();
			base.defaultDesc = "DesignatorAreaSnowClearClearDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaDelete;
		}
	}
}

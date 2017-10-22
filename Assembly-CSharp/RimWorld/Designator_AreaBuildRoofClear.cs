using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaBuildRoofClear : Designator_AreaBuildRoof
	{
		public Designator_AreaBuildRoofClear() : base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorAreaBuildRoofClear".Translate();
			base.defaultDesc = "DesignatorAreaBuildRoofClearDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofAreaClear", true);
			base.hotKey = KeyBindingDefOf.Misc11;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaDelete;
		}
	}
}

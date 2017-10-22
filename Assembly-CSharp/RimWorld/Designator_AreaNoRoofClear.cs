using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_AreaNoRoofClear : Designator_AreaNoRoof
	{
		public Designator_AreaNoRoofClear() : base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorAreaNoRoofClear".Translate();
			base.defaultDesc = "DesignatorAreaNoRoofClearDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofAreaOff", true);
			base.hotKey = KeyBindingDefOf.Misc6;
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateAreaDelete;
		}
	}
}

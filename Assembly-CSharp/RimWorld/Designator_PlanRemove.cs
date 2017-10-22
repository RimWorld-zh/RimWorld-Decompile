using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlanRemove : Designator_Plan
	{
		public Designator_PlanRemove() : base(DesignateMode.Remove)
		{
			base.defaultLabel = "DesignatorPlanRemove".Translate();
			base.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			base.soundSucceeded = SoundDefOf.DesignatePlanRemove;
			base.hotKey = KeyBindingDefOf.Misc8;
		}
	}
}

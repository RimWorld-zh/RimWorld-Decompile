using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlanAdd : Designator_Plan
	{
		public Designator_PlanAdd() : base(DesignateMode.Add)
		{
			base.defaultLabel = "DesignatorPlan".Translate();
			base.defaultDesc = "DesignatorPlanDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			base.soundSucceeded = SoundDefOf.DesignatePlanAdd;
			base.hotKey = KeyBindingDefOf.Misc9;
		}
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x06002C77 RID: 11383 RVA: 0x001769E8 File Offset: 0x00174DE8
		public Designator_PlanAdd() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorPlan".Translate();
			this.defaultDesc = "DesignatorPlanDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanAdd;
			this.hotKey = KeyBindingDefOf.Misc9;
		}
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x06002C79 RID: 11385 RVA: 0x00176A7C File Offset: 0x00174E7C
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

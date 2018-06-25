using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D4 RID: 2004
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x06002C75 RID: 11381 RVA: 0x00177008 File Offset: 0x00175408
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

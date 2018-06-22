using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D2 RID: 2002
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x06002C72 RID: 11378 RVA: 0x00176C54 File Offset: 0x00175054
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

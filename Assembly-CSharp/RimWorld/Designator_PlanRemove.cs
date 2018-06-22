using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D3 RID: 2003
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06002C73 RID: 11379 RVA: 0x00176CB0 File Offset: 0x001750B0
		public Designator_PlanRemove() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorPlanRemove".Translate();
			this.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanRemove;
			this.hotKey = KeyBindingDefOf.Misc8;
		}
	}
}

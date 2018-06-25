using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06002C77 RID: 11383 RVA: 0x00176E00 File Offset: 0x00175200
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

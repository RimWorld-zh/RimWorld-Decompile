using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D7 RID: 2007
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06002C7A RID: 11386 RVA: 0x00176AD8 File Offset: 0x00174ED8
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

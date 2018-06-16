using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D7 RID: 2007
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x06002C78 RID: 11384 RVA: 0x00176A44 File Offset: 0x00174E44
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

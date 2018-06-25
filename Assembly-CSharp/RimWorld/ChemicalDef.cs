using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000292 RID: 658
	public class ChemicalDef : Def
	{
		// Token: 0x04000593 RID: 1427
		public HediffDef addictionHediff;

		// Token: 0x04000594 RID: 1428
		public HediffDef toleranceHediff;

		// Token: 0x04000595 RID: 1429
		public bool canBinge = true;

		// Token: 0x04000596 RID: 1430
		public float onGeneratedAddictedToleranceChance = 0f;

		// Token: 0x04000597 RID: 1431
		public List<HediffGiver_Event> onGeneratedAddictedEvents = null;
	}
}

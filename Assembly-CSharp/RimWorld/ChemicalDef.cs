using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000290 RID: 656
	public class ChemicalDef : Def
	{
		// Token: 0x04000595 RID: 1429
		public HediffDef addictionHediff;

		// Token: 0x04000596 RID: 1430
		public HediffDef toleranceHediff;

		// Token: 0x04000597 RID: 1431
		public bool canBinge = true;

		// Token: 0x04000598 RID: 1432
		public float onGeneratedAddictedToleranceChance = 0f;

		// Token: 0x04000599 RID: 1433
		public List<HediffGiver_Event> onGeneratedAddictedEvents = null;
	}
}

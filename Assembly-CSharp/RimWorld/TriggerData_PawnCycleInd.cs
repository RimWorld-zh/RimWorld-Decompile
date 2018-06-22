using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AF RID: 431
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x000539D8 File Offset: 0x00051DD8
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x040003BF RID: 959
		public int pawnCycleInd;
	}
}

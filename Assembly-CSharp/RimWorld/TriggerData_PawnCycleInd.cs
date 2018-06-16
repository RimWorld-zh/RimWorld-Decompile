using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AF RID: 431
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x000539C4 File Offset: 0x00051DC4
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x040003C1 RID: 961
		public int pawnCycleInd;
	}
}

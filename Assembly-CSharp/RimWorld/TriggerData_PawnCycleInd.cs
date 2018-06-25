using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AF RID: 431
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x040003C0 RID: 960
		public int pawnCycleInd;

		// Token: 0x060008DE RID: 2270 RVA: 0x000539D4 File Offset: 0x00051DD4
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}
	}
}

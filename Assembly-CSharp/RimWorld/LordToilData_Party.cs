using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A4 RID: 420
	public class LordToilData_Party : LordToilData
	{
		// Token: 0x040003AE RID: 942
		public int ticksToNextPulse;

		// Token: 0x060008B3 RID: 2227 RVA: 0x000520C6 File Offset: 0x000504C6
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToNextPulse, "ticksToNextPulse", 0, false);
		}
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A4 RID: 420
	public class LordToilData_Party : LordToilData
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x000520DE File Offset: 0x000504DE
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToNextPulse, "ticksToNextPulse", 0, false);
		}

		// Token: 0x040003AD RID: 941
		public int ticksToNextPulse;
	}
}

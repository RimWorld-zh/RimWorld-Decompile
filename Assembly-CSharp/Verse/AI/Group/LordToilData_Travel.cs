using System;

namespace Verse.AI.Group
{
	// Token: 0x020009FA RID: 2554
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x0400247D RID: 9341
		public IntVec3 dest;

		// Token: 0x06003959 RID: 14681 RVA: 0x001E70B8 File Offset: 0x001E54B8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}
	}
}

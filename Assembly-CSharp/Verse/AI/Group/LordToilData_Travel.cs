using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F8 RID: 2552
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x06003955 RID: 14677 RVA: 0x001E6F8C File Offset: 0x001E538C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x0400247C RID: 9340
		public IntVec3 dest;
	}
}

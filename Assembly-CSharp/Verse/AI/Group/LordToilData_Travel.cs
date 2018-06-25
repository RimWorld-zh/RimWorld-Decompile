using System;

namespace Verse.AI.Group
{
	// Token: 0x020009FB RID: 2555
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x0400248D RID: 9357
		public IntVec3 dest;

		// Token: 0x0600395A RID: 14682 RVA: 0x001E73E4 File Offset: 0x001E57E4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}
	}
}

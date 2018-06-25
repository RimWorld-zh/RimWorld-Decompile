using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F2 RID: 2546
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x04002474 RID: 9332
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002475 RID: 9333
		public float defendRadius = 28f;

		// Token: 0x0600393A RID: 14650 RVA: 0x001E6CC0 File Offset: 0x001E50C0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
		}
	}
}

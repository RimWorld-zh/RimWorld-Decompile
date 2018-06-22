using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F0 RID: 2544
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x06003936 RID: 14646 RVA: 0x001E6B94 File Offset: 0x001E4F94
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
		}

		// Token: 0x04002473 RID: 9331
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002474 RID: 9332
		public float defendRadius = 28f;
	}
}

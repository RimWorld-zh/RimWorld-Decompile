using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F3 RID: 2547
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x04002484 RID: 9348
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002485 RID: 9349
		public float defendRadius = 28f;

		// Token: 0x0600393B RID: 14651 RVA: 0x001E6FEC File Offset: 0x001E53EC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
		}
	}
}

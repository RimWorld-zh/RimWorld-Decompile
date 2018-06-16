using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F4 RID: 2548
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x0600393A RID: 14650 RVA: 0x001E6880 File Offset: 0x001E4C80
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
		}

		// Token: 0x04002478 RID: 9336
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002479 RID: 9337
		public float defendRadius = 28f;
	}
}

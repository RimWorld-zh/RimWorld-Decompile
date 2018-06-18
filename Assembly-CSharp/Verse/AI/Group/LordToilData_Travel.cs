using System;

namespace Verse.AI.Group
{
	// Token: 0x020009FC RID: 2556
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x0600395B RID: 14683 RVA: 0x001E6D4C File Offset: 0x001E514C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04002481 RID: 9345
		public IntVec3 dest;
	}
}

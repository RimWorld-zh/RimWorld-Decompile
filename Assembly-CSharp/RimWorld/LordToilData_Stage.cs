using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200019E RID: 414
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x0005192C File Offset: 0x0004FD2C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x040003A2 RID: 930
		public IntVec3 stagingPoint;
	}
}

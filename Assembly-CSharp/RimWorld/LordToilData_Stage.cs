using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200019E RID: 414
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x040003A3 RID: 931
		public IntVec3 stagingPoint;

		// Token: 0x06000895 RID: 2197 RVA: 0x00051914 File Offset: 0x0004FD14
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}
	}
}

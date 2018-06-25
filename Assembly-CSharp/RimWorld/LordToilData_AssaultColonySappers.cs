using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000189 RID: 393
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x04000382 RID: 898
		public IntVec3 sapperDest = IntVec3.Invalid;

		// Token: 0x0600082B RID: 2091 RVA: 0x0004EB94 File Offset: 0x0004CF94
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000189 RID: 393
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x0600082C RID: 2092 RVA: 0x0004EB98 File Offset: 0x0004CF98
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x04000381 RID: 897
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000194 RID: 404
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x0600085D RID: 2141 RVA: 0x0004FE14 File Offset: 0x0004E214
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x0400038A RID: 906
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}

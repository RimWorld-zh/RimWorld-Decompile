using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000194 RID: 404
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x0400038B RID: 907
		public IntVec3 fallbackLocation = IntVec3.Invalid;

		// Token: 0x0600085C RID: 2140 RVA: 0x0004FE10 File Offset: 0x0004E210
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}
	}
}

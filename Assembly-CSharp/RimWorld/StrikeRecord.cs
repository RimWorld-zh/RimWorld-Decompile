using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000413 RID: 1043
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0009BA10 File Offset: 0x00099E10
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0009BA40 File Offset: 0x00099E40
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0009BA8C File Offset: 0x00099E8C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.cell,
				", ",
				this.def,
				", ",
				this.ticksGame,
				")"
			});
		}

		// Token: 0x04000AEE RID: 2798
		public IntVec3 cell;

		// Token: 0x04000AEF RID: 2799
		public int ticksGame;

		// Token: 0x04000AF0 RID: 2800
		public ThingDef def;

		// Token: 0x04000AF1 RID: 2801
		private const int StrikeRecordExpiryDays = 15;
	}
}

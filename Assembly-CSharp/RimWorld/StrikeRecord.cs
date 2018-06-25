using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000415 RID: 1045
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x04000AEF RID: 2799
		public IntVec3 cell;

		// Token: 0x04000AF0 RID: 2800
		public int ticksGame;

		// Token: 0x04000AF1 RID: 2801
		public ThingDef def;

		// Token: 0x04000AF2 RID: 2802
		private const int StrikeRecordExpiryDays = 15;

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0009BD44 File Offset: 0x0009A144
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0009BD74 File Offset: 0x0009A174
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0009BDC0 File Offset: 0x0009A1C0
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
	}
}

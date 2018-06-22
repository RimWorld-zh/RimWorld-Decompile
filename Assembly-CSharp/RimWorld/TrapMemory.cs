using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055E RID: 1374
	public class TrapMemory : IExposable
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x000E1802 File Offset: 0x000DFC02
		public TrapMemory()
		{
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000E180B File Offset: 0x000DFC0B
		public TrapMemory(IntVec3 cell, Map map, int tick)
		{
			this.loc = cell;
			this.map = map;
			this.tick = tick;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x000E182C File Offset: 0x000DFC2C
		public IntVec3 Cell
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x000E1848 File Offset: 0x000DFC48
		public int Tick
		{
			get
			{
				return this.tick;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x000E1864 File Offset: 0x000DFC64
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.tick;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x000E188C File Offset: 0x000DFC8C
		public bool Expired
		{
			get
			{
				return this.Age > 1680000;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x000E18B0 File Offset: 0x000DFCB0
		public float PowerPercent
		{
			get
			{
				return 1f - (float)this.Age / 1680000f;
			}
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x000E18D8 File Offset: 0x000DFCD8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}

		// Token: 0x04000F39 RID: 3897
		public IntVec3 loc;

		// Token: 0x04000F3A RID: 3898
		public Map map;

		// Token: 0x04000F3B RID: 3899
		public int tick;

		// Token: 0x04000F3C RID: 3900
		private const int TrapRecordTicksBeforeExpiry = 1680000;
	}
}

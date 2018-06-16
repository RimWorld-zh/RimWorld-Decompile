using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000562 RID: 1378
	public class TrapMemory : IExposable
	{
		// Token: 0x060019FB RID: 6651 RVA: 0x000E175A File Offset: 0x000DFB5A
		public TrapMemory()
		{
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000E1763 File Offset: 0x000DFB63
		public TrapMemory(IntVec3 cell, Map map, int tick)
		{
			this.loc = cell;
			this.map = map;
			this.tick = tick;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x000E1784 File Offset: 0x000DFB84
		public IntVec3 Cell
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x000E17A0 File Offset: 0x000DFBA0
		public int Tick
		{
			get
			{
				return this.tick;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x000E17BC File Offset: 0x000DFBBC
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.tick;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x000E17E4 File Offset: 0x000DFBE4
		public bool Expired
		{
			get
			{
				return this.Age > 1680000;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x000E1808 File Offset: 0x000DFC08
		public float PowerPercent
		{
			get
			{
				return 1f - (float)this.Age / 1680000f;
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x000E1830 File Offset: 0x000DFC30
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}

		// Token: 0x04000F3C RID: 3900
		public IntVec3 loc;

		// Token: 0x04000F3D RID: 3901
		public Map map;

		// Token: 0x04000F3E RID: 3902
		public int tick;

		// Token: 0x04000F3F RID: 3903
		private const int TrapRecordTicksBeforeExpiry = 1680000;
	}
}

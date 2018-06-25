using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000560 RID: 1376
	public class TrapMemory : IExposable
	{
		// Token: 0x04000F3D RID: 3901
		public IntVec3 loc;

		// Token: 0x04000F3E RID: 3902
		public Map map;

		// Token: 0x04000F3F RID: 3903
		public int tick;

		// Token: 0x04000F40 RID: 3904
		private const int TrapRecordTicksBeforeExpiry = 1680000;

		// Token: 0x060019F6 RID: 6646 RVA: 0x000E1BBA File Offset: 0x000DFFBA
		public TrapMemory()
		{
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000E1BC3 File Offset: 0x000DFFC3
		public TrapMemory(IntVec3 cell, Map map, int tick)
		{
			this.loc = cell;
			this.map = map;
			this.tick = tick;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x000E1BE4 File Offset: 0x000DFFE4
		public IntVec3 Cell
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x000E1C00 File Offset: 0x000E0000
		public int Tick
		{
			get
			{
				return this.tick;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x000E1C1C File Offset: 0x000E001C
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.tick;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x000E1C44 File Offset: 0x000E0044
		public bool Expired
		{
			get
			{
				return this.Age > 1680000;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x000E1C68 File Offset: 0x000E0068
		public float PowerPercent
		{
			get
			{
				return 1f - (float)this.Age / 1680000f;
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x000E1C90 File Offset: 0x000E0090
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}
	}
}

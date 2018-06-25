using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055F RID: 1375
	public class FactionTacticalMemory : IExposable
	{
		// Token: 0x04000F3A RID: 3898
		private List<TrapMemory> traps = new List<TrapMemory>();

		// Token: 0x04000F3B RID: 3899
		private const float TrapRememberChance = 0.2f;

		// Token: 0x060019F1 RID: 6641 RVA: 0x000E1AB1 File Offset: 0x000DFEB1
		public void ExposeData()
		{
			Scribe_Collections.Look<TrapMemory>(ref this.traps, "traps", LookMode.Deep, new object[0]);
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000E1ACC File Offset: 0x000DFECC
		public void Notify_MapRemoved(Map map)
		{
			this.traps.RemoveAll((TrapMemory x) => x.map == map);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000E1B00 File Offset: 0x000DFF00
		public List<TrapMemory> TrapMemories()
		{
			this.traps.RemoveAll((TrapMemory tl) => tl.Expired);
			return this.traps;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000E1B44 File Offset: 0x000DFF44
		public void TrapRevealed(IntVec3 c, Map map)
		{
			if (Rand.Value < 0.2f)
			{
				this.traps.Add(new TrapMemory(c, map, Find.TickManager.TicksGame));
			}
		}
	}
}

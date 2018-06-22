using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055D RID: 1373
	public class FactionTacticalMemory : IExposable
	{
		// Token: 0x060019EE RID: 6638 RVA: 0x000E16F9 File Offset: 0x000DFAF9
		public void ExposeData()
		{
			Scribe_Collections.Look<TrapMemory>(ref this.traps, "traps", LookMode.Deep, new object[0]);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000E1714 File Offset: 0x000DFB14
		public void Notify_MapRemoved(Map map)
		{
			this.traps.RemoveAll((TrapMemory x) => x.map == map);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000E1748 File Offset: 0x000DFB48
		public List<TrapMemory> TrapMemories()
		{
			this.traps.RemoveAll((TrapMemory tl) => tl.Expired);
			return this.traps;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000E178C File Offset: 0x000DFB8C
		public void TrapRevealed(IntVec3 c, Map map)
		{
			if (Rand.Value < 0.2f)
			{
				this.traps.Add(new TrapMemory(c, map, Find.TickManager.TicksGame));
			}
		}

		// Token: 0x04000F36 RID: 3894
		private List<TrapMemory> traps = new List<TrapMemory>();

		// Token: 0x04000F37 RID: 3895
		private const float TrapRememberChance = 0.2f;
	}
}

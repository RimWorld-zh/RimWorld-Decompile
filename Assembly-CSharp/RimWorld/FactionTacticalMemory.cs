using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055F RID: 1375
	public class FactionTacticalMemory : IExposable
	{
		// Token: 0x04000F36 RID: 3894
		private List<TrapMemory> traps = new List<TrapMemory>();

		// Token: 0x04000F37 RID: 3895
		private const float TrapRememberChance = 0.2f;

		// Token: 0x060019F2 RID: 6642 RVA: 0x000E1849 File Offset: 0x000DFC49
		public void ExposeData()
		{
			Scribe_Collections.Look<TrapMemory>(ref this.traps, "traps", LookMode.Deep, new object[0]);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000E1864 File Offset: 0x000DFC64
		public void Notify_MapRemoved(Map map)
		{
			this.traps.RemoveAll((TrapMemory x) => x.map == map);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000E1898 File Offset: 0x000DFC98
		public List<TrapMemory> TrapMemories()
		{
			this.traps.RemoveAll((TrapMemory tl) => tl.Expired);
			return this.traps;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000E18DC File Offset: 0x000DFCDC
		public void TrapRevealed(IntVec3 c, Map map)
		{
			if (Rand.Value < 0.2f)
			{
				this.traps.Add(new TrapMemory(c, map, Find.TickManager.TicksGame));
			}
		}
	}
}

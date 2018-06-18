using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000561 RID: 1377
	public class FactionTacticalMemory : IExposable
	{
		// Token: 0x060019F7 RID: 6647 RVA: 0x000E16A5 File Offset: 0x000DFAA5
		public void ExposeData()
		{
			Scribe_Collections.Look<TrapMemory>(ref this.traps, "traps", LookMode.Deep, new object[0]);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x000E16C0 File Offset: 0x000DFAC0
		public void Notify_MapRemoved(Map map)
		{
			this.traps.RemoveAll((TrapMemory x) => x.map == map);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x000E16F4 File Offset: 0x000DFAF4
		public List<TrapMemory> TrapMemories()
		{
			this.traps.RemoveAll((TrapMemory tl) => tl.Expired);
			return this.traps;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x000E1738 File Offset: 0x000DFB38
		public void TrapRevealed(IntVec3 c, Map map)
		{
			if (Rand.Value < 0.2f)
			{
				this.traps.Add(new TrapMemory(c, map, Find.TickManager.TicksGame));
			}
		}

		// Token: 0x04000F39 RID: 3897
		private List<TrapMemory> traps = new List<TrapMemory>();

		// Token: 0x04000F3A RID: 3898
		private const float TrapRememberChance = 0.2f;
	}
}

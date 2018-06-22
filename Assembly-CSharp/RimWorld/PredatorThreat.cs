using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000564 RID: 1380
	public class PredatorThreat : IExposable
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x000E278C File Offset: 0x000E0B8C
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000E27D3 File Offset: 0x000E0BD3
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04000F46 RID: 3910
		public Pawn predator;

		// Token: 0x04000F47 RID: 3911
		public int lastAttackTicks;

		// Token: 0x04000F48 RID: 3912
		private const int ExpireAfterTicks = 600;
	}
}

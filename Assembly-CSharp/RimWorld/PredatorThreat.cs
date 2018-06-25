using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000566 RID: 1382
	public class PredatorThreat : IExposable
	{
		// Token: 0x04000F46 RID: 3910
		public Pawn predator;

		// Token: 0x04000F47 RID: 3911
		public int lastAttackTicks;

		// Token: 0x04000F48 RID: 3912
		private const int ExpireAfterTicks = 600;

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001A1E RID: 6686 RVA: 0x000E28DC File Offset: 0x000E0CDC
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000E2923 File Offset: 0x000E0D23
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}
	}
}

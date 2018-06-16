using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000568 RID: 1384
	public class PredatorThreat : IExposable
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x000E26E4 File Offset: 0x000E0AE4
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000E272B File Offset: 0x000E0B2B
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04000F49 RID: 3913
		public Pawn predator;

		// Token: 0x04000F4A RID: 3914
		public int lastAttackTicks;

		// Token: 0x04000F4B RID: 3915
		private const int ExpireAfterTicks = 600;
	}
}

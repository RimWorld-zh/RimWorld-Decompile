using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000566 RID: 1382
	public class PredatorThreat : IExposable
	{
		// Token: 0x04000F4A RID: 3914
		public Pawn predator;

		// Token: 0x04000F4B RID: 3915
		public int lastAttackTicks;

		// Token: 0x04000F4C RID: 3916
		private const int ExpireAfterTicks = 600;

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000E2B44 File Offset: 0x000E0F44
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000E2B8B File Offset: 0x000E0F8B
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}
	}
}

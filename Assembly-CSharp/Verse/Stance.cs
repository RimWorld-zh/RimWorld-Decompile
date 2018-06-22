using System;

namespace Verse
{
	// Token: 0x02000D5B RID: 3419
	public abstract class Stance : IExposable
	{
		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x0027FE8C File Offset: 0x0027E28C
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x0027FEA4 File Offset: 0x0027E2A4
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0027FEC4 File Offset: 0x0027E2C4
		public virtual void StanceTick()
		{
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x0027FEC7 File Offset: 0x0027E2C7
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x0027FECA File Offset: 0x0027E2CA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400332A RID: 13098
		public Pawn_StanceTracker stanceTracker;
	}
}

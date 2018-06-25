using System;

namespace Verse
{
	// Token: 0x02000D5E RID: 3422
	public abstract class Stance : IExposable
	{
		// Token: 0x04003331 RID: 13105
		public Pawn_StanceTracker stanceTracker;

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00280298 File Offset: 0x0027E698
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x002802B0 File Offset: 0x0027E6B0
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x002802D0 File Offset: 0x0027E6D0
		public virtual void StanceTick()
		{
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x002802D3 File Offset: 0x0027E6D3
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x002802D6 File Offset: 0x0027E6D6
		public virtual void ExposeData()
		{
		}
	}
}

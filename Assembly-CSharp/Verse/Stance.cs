using System;

namespace Verse
{
	// Token: 0x02000D5F RID: 3423
	public abstract class Stance : IExposable
	{
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x0027E8FC File Offset: 0x0027CCFC
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x0027E914 File Offset: 0x0027CD14
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0027E934 File Offset: 0x0027CD34
		public virtual void StanceTick()
		{
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x0027E937 File Offset: 0x0027CD37
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x0027E93A File Offset: 0x0027CD3A
		public virtual void ExposeData()
		{
		}

		// Token: 0x04003321 RID: 13089
		public Pawn_StanceTracker stanceTracker;
	}
}

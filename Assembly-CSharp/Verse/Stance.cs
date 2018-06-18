using System;

namespace Verse
{
	// Token: 0x02000D5E RID: 3422
	public abstract class Stance : IExposable
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0027E8DC File Offset: 0x0027CCDC
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x0027E8F4 File Offset: 0x0027CCF4
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0027E914 File Offset: 0x0027CD14
		public virtual void StanceTick()
		{
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0027E917 File Offset: 0x0027CD17
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0027E91A File Offset: 0x0027CD1A
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400331F RID: 13087
		public Pawn_StanceTracker stanceTracker;
	}
}

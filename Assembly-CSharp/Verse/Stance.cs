using System;

namespace Verse
{
	// Token: 0x02000D5D RID: 3421
	public abstract class Stance : IExposable
	{
		// Token: 0x0400332A RID: 13098
		public Pawn_StanceTracker stanceTracker;

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x0027FFB8 File Offset: 0x0027E3B8
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0027FFD0 File Offset: 0x0027E3D0
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x0027FFF0 File Offset: 0x0027E3F0
		public virtual void StanceTick()
		{
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x0027FFF3 File Offset: 0x0027E3F3
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x0027FFF6 File Offset: 0x0027E3F6
		public virtual void ExposeData()
		{
		}
	}
}

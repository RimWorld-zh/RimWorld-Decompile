using System;

namespace Verse
{
	// Token: 0x02000BC5 RID: 3013
	public abstract class GameComponent : IExposable
	{
		// Token: 0x0600419A RID: 16794 RVA: 0x0022A424 File Offset: 0x00228824
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x0022A427 File Offset: 0x00228827
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0022A42A File Offset: 0x0022882A
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0022A42D File Offset: 0x0022882D
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0022A430 File Offset: 0x00228830
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0022A433 File Offset: 0x00228833
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0022A436 File Offset: 0x00228836
		public virtual void LoadedGame()
		{
		}
	}
}

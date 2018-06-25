using System;

namespace Verse
{
	// Token: 0x02000BC4 RID: 3012
	public abstract class GameComponent : IExposable
	{
		// Token: 0x0600419A RID: 16794 RVA: 0x0022A144 File Offset: 0x00228544
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x0022A147 File Offset: 0x00228547
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0022A14A File Offset: 0x0022854A
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0022A14D File Offset: 0x0022854D
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0022A150 File Offset: 0x00228550
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0022A153 File Offset: 0x00228553
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0022A156 File Offset: 0x00228556
		public virtual void LoadedGame()
		{
		}
	}
}

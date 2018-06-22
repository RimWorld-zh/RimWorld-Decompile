using System;

namespace Verse
{
	// Token: 0x02000BC2 RID: 3010
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06004197 RID: 16791 RVA: 0x0022A068 File Offset: 0x00228468
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0022A06B File Offset: 0x0022846B
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x0022A06E File Offset: 0x0022846E
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x0022A071 File Offset: 0x00228471
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x0022A074 File Offset: 0x00228474
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0022A077 File Offset: 0x00228477
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0022A07A File Offset: 0x0022847A
		public virtual void LoadedGame()
		{
		}
	}
}

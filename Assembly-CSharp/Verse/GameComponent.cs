using System;

namespace Verse
{
	// Token: 0x02000BC6 RID: 3014
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06004195 RID: 16789 RVA: 0x00229994 File Offset: 0x00227D94
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x00229997 File Offset: 0x00227D97
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x0022999A File Offset: 0x00227D9A
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0022999D File Offset: 0x00227D9D
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x002299A0 File Offset: 0x00227DA0
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x002299A3 File Offset: 0x00227DA3
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x002299A6 File Offset: 0x00227DA6
		public virtual void LoadedGame()
		{
		}
	}
}

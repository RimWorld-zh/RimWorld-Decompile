using System;

namespace Verse
{
	// Token: 0x02000BC6 RID: 3014
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06004193 RID: 16787 RVA: 0x0022991C File Offset: 0x00227D1C
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x0022991F File Offset: 0x00227D1F
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x00229922 File Offset: 0x00227D22
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x00229925 File Offset: 0x00227D25
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x00229928 File Offset: 0x00227D28
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0022992B File Offset: 0x00227D2B
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x0022992E File Offset: 0x00227D2E
		public virtual void LoadedGame()
		{
		}
	}
}

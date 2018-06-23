using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC4 RID: 3012
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x04002CD4 RID: 11476
		private List<Func<bool>> callbacks = new List<Func<bool>>();

		// Token: 0x060041A4 RID: 16804 RVA: 0x0022A308 File Offset: 0x00228708
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x0022A31C File Offset: 0x0022871C
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x0022A354 File Offset: 0x00228754
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC8 RID: 3016
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x060041A0 RID: 16800 RVA: 0x00229BBC File Offset: 0x00227FBC
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00229BD0 File Offset: 0x00227FD0
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x00229C08 File Offset: 0x00228008
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x04002CCF RID: 11471
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}

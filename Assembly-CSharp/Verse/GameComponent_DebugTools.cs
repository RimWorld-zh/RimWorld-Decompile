using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC8 RID: 3016
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x060041A2 RID: 16802 RVA: 0x00229C34 File Offset: 0x00228034
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x00229C48 File Offset: 0x00228048
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x00229C80 File Offset: 0x00228080
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x04002CCF RID: 11471
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}

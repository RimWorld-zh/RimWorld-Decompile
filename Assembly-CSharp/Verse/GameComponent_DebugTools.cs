using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC6 RID: 3014
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x04002CD4 RID: 11476
		private List<Func<bool>> callbacks = new List<Func<bool>>();

		// Token: 0x060041A7 RID: 16807 RVA: 0x0022A3E4 File Offset: 0x002287E4
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0022A3F8 File Offset: 0x002287F8
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0022A430 File Offset: 0x00228830
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}
	}
}

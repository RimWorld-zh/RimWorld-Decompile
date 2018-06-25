using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC7 RID: 3015
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x04002CDB RID: 11483
		private List<Func<bool>> callbacks = new List<Func<bool>>();

		// Token: 0x060041A7 RID: 16807 RVA: 0x0022A6C4 File Offset: 0x00228AC4
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0022A6D8 File Offset: 0x00228AD8
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0022A710 File Offset: 0x00228B10
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}
	}
}

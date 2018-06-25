using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBE RID: 4030
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x04003FC2 RID: 16322
		private List<T> items = new List<T>();

		// Token: 0x04003FC3 RID: 16323
		private int readIndex = 0;

		// Token: 0x0600616C RID: 24940 RVA: 0x00313A30 File Offset: 0x00311E30
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			return this.items[this.readIndex++];
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00313A89 File Offset: 0x00311E89
		public void Clear()
		{
			this.readIndex = 0;
		}
	}
}

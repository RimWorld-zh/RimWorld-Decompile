using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBD RID: 4029
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x04003FBA RID: 16314
		private List<T> items = new List<T>();

		// Token: 0x04003FBB RID: 16315
		private int readIndex = 0;

		// Token: 0x0600616C RID: 24940 RVA: 0x003137EC File Offset: 0x00311BEC
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			return this.items[this.readIndex++];
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00313845 File Offset: 0x00311C45
		public void Clear()
		{
			this.readIndex = 0;
		}
	}
}

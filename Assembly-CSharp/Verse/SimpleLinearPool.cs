using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB9 RID: 4025
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x0600615C RID: 24924 RVA: 0x00312D0C File Offset: 0x0031110C
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			return this.items[this.readIndex++];
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x00312D65 File Offset: 0x00311165
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x04003FB2 RID: 16306
		private List<T> items = new List<T>();

		// Token: 0x04003FB3 RID: 16307
		private int readIndex = 0;
	}
}

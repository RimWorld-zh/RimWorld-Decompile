using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB8 RID: 4024
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x06006133 RID: 24883 RVA: 0x00310C38 File Offset: 0x0030F038
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			return this.items[this.readIndex++];
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x00310C91 File Offset: 0x0030F091
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x04003F95 RID: 16277
		private List<T> items = new List<T>();

		// Token: 0x04003F96 RID: 16278
		private int readIndex = 0;
	}
}

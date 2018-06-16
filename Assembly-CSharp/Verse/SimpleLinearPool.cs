using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB9 RID: 4025
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x06006135 RID: 24885 RVA: 0x00310B5C File Offset: 0x0030EF5C
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			return this.items[this.readIndex++];
		}

		// Token: 0x06006136 RID: 24886 RVA: 0x00310BB5 File Offset: 0x0030EFB5
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x04003F96 RID: 16278
		private List<T> items = new List<T>();

		// Token: 0x04003F97 RID: 16279
		private int readIndex = 0;
	}
}

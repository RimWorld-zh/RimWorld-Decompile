using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE8 RID: 3816
	internal class Deque<T>
	{
		// Token: 0x06005AA7 RID: 23207 RVA: 0x002E7895 File Offset: 0x002E5C95
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x002E78B8 File Offset: 0x002E5CB8
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x002E78D8 File Offset: 0x002E5CD8
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x002E793C File Offset: 0x002E5D3C
		public void PushBack(T item)
		{
			this.PushPrep();
			this.data[(this.first + this.count++) % this.data.Length] = item;
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x002E7980 File Offset: 0x002E5D80
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x002E79E7 File Offset: 0x002E5DE7
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x002E79F8 File Offset: 0x002E5DF8
		private void PushPrep()
		{
			if (this.count >= this.data.Length)
			{
				T[] destinationArray = new T[this.data.Length * 2];
				Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
				if (this.first + this.count > this.data.Length)
				{
					Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
				}
				this.data = destinationArray;
				this.first = 0;
			}
		}

		// Token: 0x04003C98 RID: 15512
		private T[] data;

		// Token: 0x04003C99 RID: 15513
		private int first;

		// Token: 0x04003C9A RID: 15514
		private int count;
	}
}

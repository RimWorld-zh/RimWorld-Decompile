using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEA RID: 3818
	internal class Deque<T>
	{
		// Token: 0x04003C98 RID: 15512
		private T[] data;

		// Token: 0x04003C99 RID: 15513
		private int first;

		// Token: 0x04003C9A RID: 15514
		private int count;

		// Token: 0x06005AAA RID: 23210 RVA: 0x002E79B5 File Offset: 0x002E5DB5
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06005AAB RID: 23211 RVA: 0x002E79D8 File Offset: 0x002E5DD8
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x002E79F8 File Offset: 0x002E5DF8
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

		// Token: 0x06005AAD RID: 23213 RVA: 0x002E7A5C File Offset: 0x002E5E5C
		public void PushBack(T item)
		{
			this.PushPrep();
			this.data[(this.first + this.count++) % this.data.Length] = item;
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x002E7AA0 File Offset: 0x002E5EA0
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x002E7B07 File Offset: 0x002E5F07
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x002E7B18 File Offset: 0x002E5F18
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
	}
}

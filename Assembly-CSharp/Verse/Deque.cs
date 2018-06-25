using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEB RID: 3819
	internal class Deque<T>
	{
		// Token: 0x04003CA0 RID: 15520
		private T[] data;

		// Token: 0x04003CA1 RID: 15521
		private int first;

		// Token: 0x04003CA2 RID: 15522
		private int count;

		// Token: 0x06005AAA RID: 23210 RVA: 0x002E7BD5 File Offset: 0x002E5FD5
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06005AAB RID: 23211 RVA: 0x002E7BF8 File Offset: 0x002E5FF8
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x002E7C18 File Offset: 0x002E6018
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

		// Token: 0x06005AAD RID: 23213 RVA: 0x002E7C7C File Offset: 0x002E607C
		public void PushBack(T item)
		{
			this.PushPrep();
			this.data[(this.first + this.count++) % this.data.Length] = item;
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x002E7CC0 File Offset: 0x002E60C0
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x002E7D27 File Offset: 0x002E6127
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x002E7D38 File Offset: 0x002E6138
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

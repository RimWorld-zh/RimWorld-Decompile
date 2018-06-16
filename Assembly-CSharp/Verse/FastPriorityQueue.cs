using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F28 RID: 3880
	public class FastPriorityQueue<T>
	{
		// Token: 0x06005CC1 RID: 23745 RVA: 0x002EF9B2 File Offset: 0x002EDDB2
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002EF9D1 File Offset: 0x002EDDD1
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06005CC3 RID: 23747 RVA: 0x002EF9EC File Offset: 0x002EDDEC
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x002EFA0C File Offset: 0x002EDE0C
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x002EFA6C File Offset: 0x002EDE6C
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x002EFB32 File Offset: 0x002EDF32
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x002EFB40 File Offset: 0x002EDF40
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x002EFB80 File Offset: 0x002EDF80
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x04003D8D RID: 15757
		protected List<T> innerList = new List<T>();

		// Token: 0x04003D8E RID: 15758
		protected IComparer<T> comparer;
	}
}

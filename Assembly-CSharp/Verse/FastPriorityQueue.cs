using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F2B RID: 3883
	public class FastPriorityQueue<T>
	{
		// Token: 0x04003DA1 RID: 15777
		protected List<T> innerList = new List<T>();

		// Token: 0x04003DA2 RID: 15778
		protected IComparer<T> comparer;

		// Token: 0x06005CF1 RID: 23793 RVA: 0x002F213A File Offset: 0x002F053A
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x002F2159 File Offset: 0x002F0559
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005CF3 RID: 23795 RVA: 0x002F2174 File Offset: 0x002F0574
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06005CF4 RID: 23796 RVA: 0x002F2194 File Offset: 0x002F0594
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

		// Token: 0x06005CF5 RID: 23797 RVA: 0x002F21F4 File Offset: 0x002F05F4
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

		// Token: 0x06005CF6 RID: 23798 RVA: 0x002F22BA File Offset: 0x002F06BA
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06005CF7 RID: 23799 RVA: 0x002F22C8 File Offset: 0x002F06C8
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06005CF8 RID: 23800 RVA: 0x002F2308 File Offset: 0x002F0708
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}
	}
}

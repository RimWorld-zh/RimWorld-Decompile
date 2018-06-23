using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F27 RID: 3879
	public class FastPriorityQueue<T>
	{
		// Token: 0x04003D9E RID: 15774
		protected List<T> innerList = new List<T>();

		// Token: 0x04003D9F RID: 15775
		protected IComparer<T> comparer;

		// Token: 0x06005CE7 RID: 23783 RVA: 0x002F1ABA File Offset: 0x002EFEBA
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x002F1AD9 File Offset: 0x002EFED9
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x002F1AF4 File Offset: 0x002EFEF4
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x002F1B14 File Offset: 0x002EFF14
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

		// Token: 0x06005CEB RID: 23787 RVA: 0x002F1B74 File Offset: 0x002EFF74
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

		// Token: 0x06005CEC RID: 23788 RVA: 0x002F1C3A File Offset: 0x002F003A
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x002F1C48 File Offset: 0x002F0048
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x002F1C88 File Offset: 0x002F0088
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}
	}
}

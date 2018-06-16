using System;

namespace Verse
{
	// Token: 0x02000C80 RID: 3200
	public class FloodFillRangeQueue
	{
		// Token: 0x060045FC RID: 17916 RVA: 0x0024D41B File Offset: 0x0024B81B
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x0024D44C File Offset: 0x0024B84C
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x0024D468 File Offset: 0x0024B868
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x0024D494 File Offset: 0x0024B894
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new object[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded,
					", MaxUsedSize= ",
					this.debugMaxUsedSpace,
					", ClaimedSize=",
					this.array.Length,
					", UnusedSpace=",
					this.array.Length - this.debugMaxUsedSpace
				});
			}
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x0024D518 File Offset: 0x0024B918
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			this.array[this.head + this.count++] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0024D5C4 File Offset: 0x0024B9C4
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04002FB1 RID: 12209
		private FloodFillRange[] array;

		// Token: 0x04002FB2 RID: 12210
		private int count;

		// Token: 0x04002FB3 RID: 12211
		private int head;

		// Token: 0x04002FB4 RID: 12212
		private int debugNumTimesExpanded = 0;

		// Token: 0x04002FB5 RID: 12213
		private int debugMaxUsedSpace = 0;
	}
}

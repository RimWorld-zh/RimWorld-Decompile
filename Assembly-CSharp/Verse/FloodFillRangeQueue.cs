using System;

namespace Verse
{
	// Token: 0x02000C7F RID: 3199
	public class FloodFillRangeQueue
	{
		// Token: 0x060045FA RID: 17914 RVA: 0x0024D3F3 File Offset: 0x0024B7F3
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x0024D424 File Offset: 0x0024B824
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x0024D440 File Offset: 0x0024B840
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x0024D46C File Offset: 0x0024B86C
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

		// Token: 0x060045FE RID: 17918 RVA: 0x0024D4F0 File Offset: 0x0024B8F0
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

		// Token: 0x060045FF RID: 17919 RVA: 0x0024D59C File Offset: 0x0024B99C
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

		// Token: 0x04002FAF RID: 12207
		private FloodFillRange[] array;

		// Token: 0x04002FB0 RID: 12208
		private int count;

		// Token: 0x04002FB1 RID: 12209
		private int head;

		// Token: 0x04002FB2 RID: 12210
		private int debugNumTimesExpanded = 0;

		// Token: 0x04002FB3 RID: 12211
		private int debugMaxUsedSpace = 0;
	}
}

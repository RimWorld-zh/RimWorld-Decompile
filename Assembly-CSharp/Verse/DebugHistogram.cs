using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E24 RID: 3620
	public class DebugHistogram
	{
		// Token: 0x06005505 RID: 21765 RVA: 0x002BA41A File Offset: 0x002B881A
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x002BA44C File Offset: 0x002B884C
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					break;
				}
			}
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x002BA49C File Offset: 0x002B889C
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x002BA4C4 File Offset: 0x002B88C4
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x04003811 RID: 14353
		private float[] buckets;

		// Token: 0x04003812 RID: 14354
		private int[] counts;
	}
}

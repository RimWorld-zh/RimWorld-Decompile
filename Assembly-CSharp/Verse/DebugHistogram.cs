using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E26 RID: 3622
	public class DebugHistogram
	{
		// Token: 0x04003811 RID: 14353
		private float[] buckets;

		// Token: 0x04003812 RID: 14354
		private int[] counts;

		// Token: 0x06005509 RID: 21769 RVA: 0x002BA546 File Offset: 0x002B8946
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x002BA578 File Offset: 0x002B8978
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

		// Token: 0x0600550B RID: 21771 RVA: 0x002BA5C8 File Offset: 0x002B89C8
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x002BA5F0 File Offset: 0x002B89F0
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
	}
}

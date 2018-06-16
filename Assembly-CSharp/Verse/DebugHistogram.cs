using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E28 RID: 3624
	public class DebugHistogram
	{
		// Token: 0x060054EB RID: 21739 RVA: 0x002B8862 File Offset: 0x002B6C62
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x002B8894 File Offset: 0x002B6C94
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

		// Token: 0x060054ED RID: 21741 RVA: 0x002B88E4 File Offset: 0x002B6CE4
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x002B890C File Offset: 0x002B6D0C
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

		// Token: 0x04003805 RID: 14341
		private float[] buckets;

		// Token: 0x04003806 RID: 14342
		private int[] counts;
	}
}

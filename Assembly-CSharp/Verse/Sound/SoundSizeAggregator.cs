using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB9 RID: 3513
	public class SoundSizeAggregator
	{
		// Token: 0x04003448 RID: 13384
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04003449 RID: 13385
		private float testSize;

		// Token: 0x06004E84 RID: 20100 RVA: 0x00290BD7 File Offset: 0x0028EFD7
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004E85 RID: 20101 RVA: 0x00290C10 File Offset: 0x0028F010
		public float AggregateSize
		{
			get
			{
				float result;
				if (this.reporters.Count == 0)
				{
					result = this.testSize;
				}
				else
				{
					float num = 0f;
					foreach (ISizeReporter sizeReporter in this.reporters)
					{
						num += sizeReporter.CurrentSize();
					}
					result = num;
				}
				return result;
			}
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00290C9C File Offset: 0x0028F09C
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00290CAB File Offset: 0x0028F0AB
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}
	}
}

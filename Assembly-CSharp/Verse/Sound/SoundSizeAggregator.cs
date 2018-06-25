using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB8 RID: 3512
	public class SoundSizeAggregator
	{
		// Token: 0x04003441 RID: 13377
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04003442 RID: 13378
		private float testSize;

		// Token: 0x06004E84 RID: 20100 RVA: 0x002908F7 File Offset: 0x0028ECF7
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004E85 RID: 20101 RVA: 0x00290930 File Offset: 0x0028ED30
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

		// Token: 0x06004E86 RID: 20102 RVA: 0x002909BC File Offset: 0x0028EDBC
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x002909CB File Offset: 0x0028EDCB
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}
	}
}

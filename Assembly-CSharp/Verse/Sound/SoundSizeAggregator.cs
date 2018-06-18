using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB9 RID: 3513
	public class SoundSizeAggregator
	{
		// Token: 0x06004E6B RID: 20075 RVA: 0x0028F21B File Offset: 0x0028D61B
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x0028F254 File Offset: 0x0028D654
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

		// Token: 0x06004E6D RID: 20077 RVA: 0x0028F2E0 File Offset: 0x0028D6E0
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x0028F2EF File Offset: 0x0028D6EF
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04003436 RID: 13366
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04003437 RID: 13367
		private float testSize;
	}
}

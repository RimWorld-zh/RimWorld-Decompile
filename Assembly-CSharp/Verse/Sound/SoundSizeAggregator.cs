using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB6 RID: 3510
	public class SoundSizeAggregator
	{
		// Token: 0x06004E80 RID: 20096 RVA: 0x002907CB File Offset: 0x0028EBCB
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00290804 File Offset: 0x0028EC04
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

		// Token: 0x06004E82 RID: 20098 RVA: 0x00290890 File Offset: 0x0028EC90
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0029089F File Offset: 0x0028EC9F
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04003441 RID: 13377
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04003442 RID: 13378
		private float testSize;
	}
}

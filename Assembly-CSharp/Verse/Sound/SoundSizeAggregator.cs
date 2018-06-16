using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DBA RID: 3514
	public class SoundSizeAggregator
	{
		// Token: 0x06004E6D RID: 20077 RVA: 0x0028F23B File Offset: 0x0028D63B
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x0028F274 File Offset: 0x0028D674
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

		// Token: 0x06004E6F RID: 20079 RVA: 0x0028F300 File Offset: 0x0028D700
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x0028F30F File Offset: 0x0028D70F
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04003438 RID: 13368
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04003439 RID: 13369
		private float testSize;
	}
}

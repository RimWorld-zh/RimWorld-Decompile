using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	public class SoundSizeAggregator
	{
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		private float testSize;

		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

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

		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}
	}
}

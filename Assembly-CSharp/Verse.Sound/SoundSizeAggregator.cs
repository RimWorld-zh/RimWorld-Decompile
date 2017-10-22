using System.Collections.Generic;

namespace Verse.Sound
{
	public class SoundSizeAggregator
	{
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		private float testSize;

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
					foreach (ISizeReporter reporter in this.reporters)
					{
						num += reporter.CurrentSize();
					}
					result = num;
				}
				return result;
			}
		}

		public SoundSizeAggregator()
		{
			this.testSize = (float)(Rand.Value * 3.0);
			this.testSize *= this.testSize;
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

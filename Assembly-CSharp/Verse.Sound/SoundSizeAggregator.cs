using System;
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
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				List<ISizeReporter>.Enumerator enumerator = this.reporters.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ISizeReporter current = enumerator.Current;
						num += current.CurrentSize();
					}
					return num;
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
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

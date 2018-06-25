using System;

namespace Verse.Sound
{
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		public SoundParamSource_AggregateSize()
		{
		}

		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		public override float ValueFor(Sample samp)
		{
			float result;
			if (samp.ExternalParams.sizeAggregator == null)
			{
				result = 0f;
			}
			else
			{
				result = samp.ExternalParams.sizeAggregator.AggregateSize;
			}
			return result;
		}
	}
}

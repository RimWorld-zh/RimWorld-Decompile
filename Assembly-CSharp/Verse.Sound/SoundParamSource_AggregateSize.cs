namespace Verse.Sound
{
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		public override float ValueFor(Sample samp)
		{
			return (float)((samp.ExternalParams.sizeAggregator != null) ? samp.ExternalParams.sizeAggregator.AggregateSize : 0.0);
		}
	}
}

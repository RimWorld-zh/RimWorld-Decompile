using System.Collections.Generic;

namespace Verse.Sound
{
	public class AudioGrain_Silence : AudioGrain
	{
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return (ResolvedGrain)new ResolvedGrain_Silence(this);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}
	}
}

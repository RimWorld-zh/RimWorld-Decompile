using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.Sound
{
	public class AudioGrain_Silence : AudioGrain
	{
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		[DebuggerHidden]
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioGrain_Silence.<GetResolvedGrains>c__Iterator1DD <GetResolvedGrains>c__Iterator1DD = new AudioGrain_Silence.<GetResolvedGrains>c__Iterator1DD();
			<GetResolvedGrains>c__Iterator1DD.<>f__this = this;
			AudioGrain_Silence.<GetResolvedGrains>c__Iterator1DD expr_0E = <GetResolvedGrains>c__Iterator1DD;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}
	}
}

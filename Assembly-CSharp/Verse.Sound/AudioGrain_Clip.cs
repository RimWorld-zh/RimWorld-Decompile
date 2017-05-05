using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.Sound
{
	public class AudioGrain_Clip : AudioGrain
	{
		public string clipPath = string.Empty;

		[DebuggerHidden]
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioGrain_Clip.<GetResolvedGrains>c__Iterator1D7 <GetResolvedGrains>c__Iterator1D = new AudioGrain_Clip.<GetResolvedGrains>c__Iterator1D7();
			<GetResolvedGrains>c__Iterator1D.<>f__this = this;
			AudioGrain_Clip.<GetResolvedGrains>c__Iterator1D7 expr_0E = <GetResolvedGrains>c__Iterator1D;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}

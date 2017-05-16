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
			AudioGrain_Clip.<GetResolvedGrains>c__Iterator1DB <GetResolvedGrains>c__Iterator1DB = new AudioGrain_Clip.<GetResolvedGrains>c__Iterator1DB();
			<GetResolvedGrains>c__Iterator1DB.<>f__this = this;
			AudioGrain_Clip.<GetResolvedGrains>c__Iterator1DB expr_0E = <GetResolvedGrains>c__Iterator1DB;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}

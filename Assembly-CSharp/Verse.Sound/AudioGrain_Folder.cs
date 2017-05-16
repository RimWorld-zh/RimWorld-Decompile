using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.Sound
{
	public class AudioGrain_Folder : AudioGrain
	{
		[LoadAlias("clipPath")]
		public string clipFolderPath = string.Empty;

		[DebuggerHidden]
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioGrain_Folder.<GetResolvedGrains>c__Iterator1DC <GetResolvedGrains>c__Iterator1DC = new AudioGrain_Folder.<GetResolvedGrains>c__Iterator1DC();
			<GetResolvedGrains>c__Iterator1DC.<>f__this = this;
			AudioGrain_Folder.<GetResolvedGrains>c__Iterator1DC expr_0E = <GetResolvedGrains>c__Iterator1DC;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}

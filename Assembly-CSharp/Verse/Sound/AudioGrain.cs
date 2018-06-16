using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B77 RID: 2935
	public abstract class AudioGrain
	{
		// Token: 0x06003FF7 RID: 16375
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}

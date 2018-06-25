using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B75 RID: 2933
	public abstract class AudioGrain
	{
		// Token: 0x06003FFE RID: 16382
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}

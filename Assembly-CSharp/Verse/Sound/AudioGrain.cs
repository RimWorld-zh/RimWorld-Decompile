using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000B73 RID: 2931
	public abstract class AudioGrain
	{
		// Token: 0x06003FFB RID: 16379
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}

using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	public abstract class AudioGrain
	{
		protected AudioGrain()
		{
		}

		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}

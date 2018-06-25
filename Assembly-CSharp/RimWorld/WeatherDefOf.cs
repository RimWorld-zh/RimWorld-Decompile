using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094B RID: 2379
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x04002263 RID: 8803
		public static WeatherDef Clear;

		// Token: 0x06003655 RID: 13909 RVA: 0x001D0DAF File Offset: 0x001CF1AF
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}
	}
}

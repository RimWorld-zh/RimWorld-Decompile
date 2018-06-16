using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094D RID: 2381
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x06003656 RID: 13910 RVA: 0x001D09BF File Offset: 0x001CEDBF
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04002264 RID: 8804
		public static WeatherDef Clear;
	}
}

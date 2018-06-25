using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094B RID: 2379
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x0400226A RID: 8810
		public static WeatherDef Clear;

		// Token: 0x06003655 RID: 13909 RVA: 0x001D1083 File Offset: 0x001CF483
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}
	}
}

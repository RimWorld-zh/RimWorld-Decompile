using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000949 RID: 2377
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x06003651 RID: 13905 RVA: 0x001D0C6F File Offset: 0x001CF06F
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04002262 RID: 8802
		public static WeatherDef Clear;
	}
}

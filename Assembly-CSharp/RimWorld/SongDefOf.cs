using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000941 RID: 2369
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x0600364C RID: 13900 RVA: 0x001D09AF File Offset: 0x001CEDAF
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x0400215D RID: 8541
		public static SongDef EntrySong;

		// Token: 0x0400215E RID: 8542
		public static SongDef EndCreditsSong;
	}
}

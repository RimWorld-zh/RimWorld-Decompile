using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093D RID: 2365
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x06003645 RID: 13893 RVA: 0x001D0B97 File Offset: 0x001CEF97
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x0400215B RID: 8539
		public static SongDef EntrySong;

		// Token: 0x0400215C RID: 8540
		public static SongDef EndCreditsSong;
	}
}

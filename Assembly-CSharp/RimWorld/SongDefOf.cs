using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093F RID: 2367
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x0400215C RID: 8540
		public static SongDef EntrySong;

		// Token: 0x0400215D RID: 8541
		public static SongDef EndCreditsSong;

		// Token: 0x06003649 RID: 13897 RVA: 0x001D0CD7 File Offset: 0x001CF0D7
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093F RID: 2367
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x04002163 RID: 8547
		public static SongDef EntrySong;

		// Token: 0x04002164 RID: 8548
		public static SongDef EndCreditsSong;

		// Token: 0x06003649 RID: 13897 RVA: 0x001D0FAB File Offset: 0x001CF3AB
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}
	}
}

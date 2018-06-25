using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095E RID: 2398
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x040022B9 RID: 8889
		public static LetterDef ThreatBig;

		// Token: 0x040022BA RID: 8890
		public static LetterDef ThreatSmall;

		// Token: 0x040022BB RID: 8891
		public static LetterDef NegativeEvent;

		// Token: 0x040022BC RID: 8892
		public static LetterDef NeutralEvent;

		// Token: 0x040022BD RID: 8893
		public static LetterDef PositiveEvent;

		// Token: 0x040022BE RID: 8894
		public static LetterDef Death;

		// Token: 0x06003668 RID: 13928 RVA: 0x001D11D9 File Offset: 0x001CF5D9
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}
	}
}

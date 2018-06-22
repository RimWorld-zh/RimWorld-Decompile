using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x06003664 RID: 13924 RVA: 0x001D0DC5 File Offset: 0x001CF1C5
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x040022B1 RID: 8881
		public static LetterDef ThreatBig;

		// Token: 0x040022B2 RID: 8882
		public static LetterDef ThreatSmall;

		// Token: 0x040022B3 RID: 8883
		public static LetterDef NegativeEvent;

		// Token: 0x040022B4 RID: 8884
		public static LetterDef NeutralEvent;

		// Token: 0x040022B5 RID: 8885
		public static LetterDef PositiveEvent;

		// Token: 0x040022B6 RID: 8886
		public static LetterDef Death;
	}
}

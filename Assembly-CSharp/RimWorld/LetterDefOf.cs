using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x001D0B15 File Offset: 0x001CEF15
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x040022B3 RID: 8883
		public static LetterDef ThreatBig;

		// Token: 0x040022B4 RID: 8884
		public static LetterDef ThreatSmall;

		// Token: 0x040022B5 RID: 8885
		public static LetterDef NegativeEvent;

		// Token: 0x040022B6 RID: 8886
		public static LetterDef NeutralEvent;

		// Token: 0x040022B7 RID: 8887
		public static LetterDef PositiveEvent;

		// Token: 0x040022B8 RID: 8888
		public static LetterDef Death;
	}
}

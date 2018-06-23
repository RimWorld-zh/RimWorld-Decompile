using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093E RID: 2366
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x0400215D RID: 8541
		public static RoomStatDef Cleanliness;

		// Token: 0x0400215E RID: 8542
		public static RoomStatDef Wealth;

		// Token: 0x0400215F RID: 8543
		public static RoomStatDef Space;

		// Token: 0x04002160 RID: 8544
		public static RoomStatDef Beauty;

		// Token: 0x04002161 RID: 8545
		public static RoomStatDef Impressiveness;

		// Token: 0x04002162 RID: 8546
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x04002163 RID: 8547
		public static RoomStatDef ResearchSpeedFactor;

		// Token: 0x04002164 RID: 8548
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x04002165 RID: 8549
		public static RoomStatDef FoodPoisonChanceFactor;

		// Token: 0x06003646 RID: 13894 RVA: 0x001D0BA9 File Offset: 0x001CEFA9
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}
	}
}

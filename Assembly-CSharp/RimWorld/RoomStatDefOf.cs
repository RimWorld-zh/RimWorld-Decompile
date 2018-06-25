using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000940 RID: 2368
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x04002165 RID: 8549
		public static RoomStatDef Cleanliness;

		// Token: 0x04002166 RID: 8550
		public static RoomStatDef Wealth;

		// Token: 0x04002167 RID: 8551
		public static RoomStatDef Space;

		// Token: 0x04002168 RID: 8552
		public static RoomStatDef Beauty;

		// Token: 0x04002169 RID: 8553
		public static RoomStatDef Impressiveness;

		// Token: 0x0400216A RID: 8554
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x0400216B RID: 8555
		public static RoomStatDef ResearchSpeedFactor;

		// Token: 0x0400216C RID: 8556
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x0400216D RID: 8557
		public static RoomStatDef FoodPoisonChanceFactor;

		// Token: 0x0600364A RID: 13898 RVA: 0x001D0FBD File Offset: 0x001CF3BD
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}
	}
}

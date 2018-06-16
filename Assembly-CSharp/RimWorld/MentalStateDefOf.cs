using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092E RID: 2350
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x001D0791 File Offset: 0x001CEB91
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x04001FFC RID: 8188
		public static MentalStateDef Berserk;

		// Token: 0x04001FFD RID: 8189
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x04001FFE RID: 8190
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04001FFF RID: 8191
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x04002000 RID: 8192
		public static MentalStateDef Wander_Sad;

		// Token: 0x04002001 RID: 8193
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x04002002 RID: 8194
		public static MentalStateDef PanicFlee;

		// Token: 0x04002003 RID: 8195
		public static MentalStateDef Manhunter;

		// Token: 0x04002004 RID: 8196
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x04002005 RID: 8197
		public static MentalStateDef SocialFighting;
	}
}

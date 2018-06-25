using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092C RID: 2348
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x04002001 RID: 8193
		public static MentalStateDef Berserk;

		// Token: 0x04002002 RID: 8194
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x04002003 RID: 8195
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04002004 RID: 8196
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x04002005 RID: 8197
		public static MentalStateDef Wander_Sad;

		// Token: 0x04002006 RID: 8198
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x04002007 RID: 8199
		public static MentalStateDef PanicFlee;

		// Token: 0x04002008 RID: 8200
		public static MentalStateDef Manhunter;

		// Token: 0x04002009 RID: 8201
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x0400200A RID: 8202
		public static MentalStateDef SocialFighting;

		// Token: 0x06003636 RID: 13878 RVA: 0x001D0E55 File Offset: 0x001CF255
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}
	}
}

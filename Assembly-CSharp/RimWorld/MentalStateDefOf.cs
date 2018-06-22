using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x06003632 RID: 13874 RVA: 0x001D0A41 File Offset: 0x001CEE41
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x04001FFA RID: 8186
		public static MentalStateDef Berserk;

		// Token: 0x04001FFB RID: 8187
		public static MentalStateDef Binging_DrugExtreme;

		// Token: 0x04001FFC RID: 8188
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04001FFD RID: 8189
		public static MentalStateDef Binging_DrugMajor;

		// Token: 0x04001FFE RID: 8190
		public static MentalStateDef Wander_Sad;

		// Token: 0x04001FFF RID: 8191
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x04002000 RID: 8192
		public static MentalStateDef PanicFlee;

		// Token: 0x04002001 RID: 8193
		public static MentalStateDef Manhunter;

		// Token: 0x04002002 RID: 8194
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x04002003 RID: 8195
		public static MentalStateDef SocialFighting;
	}
}

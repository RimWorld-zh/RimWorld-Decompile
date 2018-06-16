using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x0600363A RID: 13882 RVA: 0x001D07C7 File Offset: 0x001CEBC7
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x04002028 RID: 8232
		public static BodyPartGroupDef Torso;

		// Token: 0x04002029 RID: 8233
		public static BodyPartGroupDef Legs;

		// Token: 0x0400202A RID: 8234
		public static BodyPartGroupDef LeftHand;

		// Token: 0x0400202B RID: 8235
		public static BodyPartGroupDef RightHand;

		// Token: 0x0400202C RID: 8236
		public static BodyPartGroupDef FullHead;

		// Token: 0x0400202D RID: 8237
		public static BodyPartGroupDef UpperHead;

		// Token: 0x0400202E RID: 8238
		public static BodyPartGroupDef Eyes;
	}
}

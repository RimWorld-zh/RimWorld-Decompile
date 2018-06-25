using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092F RID: 2351
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x0400202D RID: 8237
		public static BodyPartGroupDef Torso;

		// Token: 0x0400202E RID: 8238
		public static BodyPartGroupDef Legs;

		// Token: 0x0400202F RID: 8239
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04002030 RID: 8240
		public static BodyPartGroupDef RightHand;

		// Token: 0x04002031 RID: 8241
		public static BodyPartGroupDef FullHead;

		// Token: 0x04002032 RID: 8242
		public static BodyPartGroupDef UpperHead;

		// Token: 0x04002033 RID: 8243
		public static BodyPartGroupDef Eyes;

		// Token: 0x06003639 RID: 13881 RVA: 0x001D0E8B File Offset: 0x001CF28B
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092D RID: 2349
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x04002026 RID: 8230
		public static BodyPartGroupDef Torso;

		// Token: 0x04002027 RID: 8231
		public static BodyPartGroupDef Legs;

		// Token: 0x04002028 RID: 8232
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04002029 RID: 8233
		public static BodyPartGroupDef RightHand;

		// Token: 0x0400202A RID: 8234
		public static BodyPartGroupDef FullHead;

		// Token: 0x0400202B RID: 8235
		public static BodyPartGroupDef UpperHead;

		// Token: 0x0400202C RID: 8236
		public static BodyPartGroupDef Eyes;

		// Token: 0x06003635 RID: 13877 RVA: 0x001D0A77 File Offset: 0x001CEE77
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}
	}
}

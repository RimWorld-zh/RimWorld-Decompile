using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	[DefOf]
	public static class BodyPartDefOf
	{
		// Token: 0x0400202D RID: 8237
		public static BodyPartDef Heart;

		// Token: 0x0400202E RID: 8238
		public static BodyPartDef Leg;

		// Token: 0x0400202F RID: 8239
		public static BodyPartDef Liver;

		// Token: 0x04002030 RID: 8240
		public static BodyPartDef Brain;

		// Token: 0x04002031 RID: 8241
		public static BodyPartDef Eye;

		// Token: 0x04002032 RID: 8242
		public static BodyPartDef Arm;

		// Token: 0x04002033 RID: 8243
		public static BodyPartDef Jaw;

		// Token: 0x04002034 RID: 8244
		public static BodyPartDef Hand;

		// Token: 0x04002035 RID: 8245
		public static BodyPartDef Neck;

		// Token: 0x04002036 RID: 8246
		public static BodyPartDef Head;

		// Token: 0x04002037 RID: 8247
		public static BodyPartDef Body;

		// Token: 0x04002038 RID: 8248
		public static BodyPartDef Torso;

		// Token: 0x04002039 RID: 8249
		public static BodyPartDef InsectHead;

		// Token: 0x0400203A RID: 8250
		public static BodyPartDef Stomach;

		// Token: 0x0600363A RID: 13882 RVA: 0x001D0BC9 File Offset: 0x001CEFC9
		static BodyPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartDefOf));
		}
	}
}

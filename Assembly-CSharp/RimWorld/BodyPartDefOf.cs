using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	[DefOf]
	public static class BodyPartDefOf
	{
		// Token: 0x04002034 RID: 8244
		public static BodyPartDef Heart;

		// Token: 0x04002035 RID: 8245
		public static BodyPartDef Leg;

		// Token: 0x04002036 RID: 8246
		public static BodyPartDef Liver;

		// Token: 0x04002037 RID: 8247
		public static BodyPartDef Brain;

		// Token: 0x04002038 RID: 8248
		public static BodyPartDef Eye;

		// Token: 0x04002039 RID: 8249
		public static BodyPartDef Arm;

		// Token: 0x0400203A RID: 8250
		public static BodyPartDef Jaw;

		// Token: 0x0400203B RID: 8251
		public static BodyPartDef Hand;

		// Token: 0x0400203C RID: 8252
		public static BodyPartDef Neck;

		// Token: 0x0400203D RID: 8253
		public static BodyPartDef Head;

		// Token: 0x0400203E RID: 8254
		public static BodyPartDef Body;

		// Token: 0x0400203F RID: 8255
		public static BodyPartDef Torso;

		// Token: 0x04002040 RID: 8256
		public static BodyPartDef InsectHead;

		// Token: 0x04002041 RID: 8257
		public static BodyPartDef Stomach;

		// Token: 0x0600363A RID: 13882 RVA: 0x001D0E9D File Offset: 0x001CF29D
		static BodyPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartDefOf));
		}
	}
}

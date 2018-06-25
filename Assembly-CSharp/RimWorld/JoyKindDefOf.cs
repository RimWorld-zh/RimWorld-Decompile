using System;

namespace RimWorld
{
	// Token: 0x02000924 RID: 2340
	[DefOf]
	public static class JoyKindDefOf
	{
		// Token: 0x04001FB2 RID: 8114
		public static JoyKindDef Meditative;

		// Token: 0x04001FB3 RID: 8115
		public static JoyKindDef Social;

		// Token: 0x04001FB4 RID: 8116
		public static JoyKindDef Gluttonous;

		// Token: 0x0600362E RID: 13870 RVA: 0x001D0AF1 File Offset: 0x001CEEF1
		static JoyKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JoyKindDefOf));
		}
	}
}

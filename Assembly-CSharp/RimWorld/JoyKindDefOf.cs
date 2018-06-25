using System;

namespace RimWorld
{
	// Token: 0x02000924 RID: 2340
	[DefOf]
	public static class JoyKindDefOf
	{
		// Token: 0x04001FB9 RID: 8121
		public static JoyKindDef Meditative;

		// Token: 0x04001FBA RID: 8122
		public static JoyKindDef Social;

		// Token: 0x04001FBB RID: 8123
		public static JoyKindDef Gluttonous;

		// Token: 0x0600362E RID: 13870 RVA: 0x001D0DC5 File Offset: 0x001CF1C5
		static JoyKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JoyKindDefOf));
		}
	}
}

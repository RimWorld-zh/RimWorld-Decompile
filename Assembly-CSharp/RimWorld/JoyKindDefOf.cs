using System;

namespace RimWorld
{
	// Token: 0x02000922 RID: 2338
	[DefOf]
	public static class JoyKindDefOf
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x001D09B1 File Offset: 0x001CEDB1
		static JoyKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JoyKindDefOf));
		}

		// Token: 0x04001FB2 RID: 8114
		public static JoyKindDef Meditative;

		// Token: 0x04001FB3 RID: 8115
		public static JoyKindDef Social;

		// Token: 0x04001FB4 RID: 8116
		public static JoyKindDef Gluttonous;
	}
}

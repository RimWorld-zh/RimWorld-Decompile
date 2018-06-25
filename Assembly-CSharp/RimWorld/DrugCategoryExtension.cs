using System;

namespace RimWorld
{
	// Token: 0x0200026A RID: 618
	public static class DrugCategoryExtension
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x00060438 File Offset: 0x0005E838
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}

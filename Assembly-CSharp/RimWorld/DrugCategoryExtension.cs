using System;

namespace RimWorld
{
	// Token: 0x0200026A RID: 618
	public static class DrugCategoryExtension
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x00060434 File Offset: 0x0005E834
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}

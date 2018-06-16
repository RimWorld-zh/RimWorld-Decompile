using System;

namespace RimWorld
{
	// Token: 0x02000268 RID: 616
	public static class DrugCategoryExtension
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x0006028C File Offset: 0x0005E68C
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}

using System;

namespace RimWorld
{
	// Token: 0x02000268 RID: 616
	public static class DrugCategoryExtension
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x000602E8 File Offset: 0x0005E6E8
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}

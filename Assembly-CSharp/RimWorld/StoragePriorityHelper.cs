using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000694 RID: 1684
	public static class StoragePriorityHelper
	{
		// Token: 0x060023B2 RID: 9138 RVA: 0x0013273C File Offset: 0x00130B3C
		public static string Label(this StoragePriority p)
		{
			string result;
			switch (p)
			{
			case StoragePriority.Unstored:
				result = "StoragePriorityUnstored".Translate();
				break;
			case StoragePriority.Low:
				result = "StoragePriorityLow".Translate();
				break;
			case StoragePriority.Normal:
				result = "StoragePriorityNormal".Translate();
				break;
			case StoragePriority.Preferred:
				result = "StoragePriorityPreferred".Translate();
				break;
			case StoragePriority.Important:
				result = "StoragePriorityImportant".Translate();
				break;
			case StoragePriority.Critical:
				result = "StoragePriorityCritical".Translate();
				break;
			default:
				result = "Unknown";
				break;
			}
			return result;
		}
	}
}

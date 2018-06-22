using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000692 RID: 1682
	public static class StoragePriorityHelper
	{
		// Token: 0x060023AF RID: 9135 RVA: 0x00132384 File Offset: 0x00130784
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

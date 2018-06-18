using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000696 RID: 1686
	public static class StoragePriorityHelper
	{
		// Token: 0x060023B7 RID: 9143 RVA: 0x0013223C File Offset: 0x0013063C
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

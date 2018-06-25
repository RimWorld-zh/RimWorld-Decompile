using System;
using Verse;

namespace RimWorld
{
	public static class StoragePriorityHelper
	{
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

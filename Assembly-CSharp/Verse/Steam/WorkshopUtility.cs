using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FCD RID: 4045
	internal static class WorkshopUtility
	{
		// Token: 0x060061C4 RID: 25028 RVA: 0x00315408 File Offset: 0x00313808
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			string result;
			if (stage == WorkshopInteractStage.None)
			{
				result = "None".Translate();
			}
			else
			{
				result = ("WorkshopInteractStage_" + stage.ToString()).Translate();
			}
			return result;
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x00315450 File Offset: 0x00313850
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x00315484 File Offset: 0x00313884
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}

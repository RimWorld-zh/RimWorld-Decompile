using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC8 RID: 4040
	internal static class WorkshopUtility
	{
		// Token: 0x060061B4 RID: 25012 RVA: 0x003146E4 File Offset: 0x00312AE4
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

		// Token: 0x060061B5 RID: 25013 RVA: 0x0031472C File Offset: 0x00312B2C
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x00314760 File Offset: 0x00312B60
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}

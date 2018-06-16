using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC8 RID: 4040
	internal static class WorkshopUtility
	{
		// Token: 0x0600618D RID: 24973 RVA: 0x00312534 File Offset: 0x00310934
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

		// Token: 0x0600618E RID: 24974 RVA: 0x0031257C File Offset: 0x0031097C
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x003125B0 File Offset: 0x003109B0
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}

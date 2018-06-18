using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC7 RID: 4039
	internal static class WorkshopUtility
	{
		// Token: 0x0600618B RID: 24971 RVA: 0x00312610 File Offset: 0x00310A10
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

		// Token: 0x0600618C RID: 24972 RVA: 0x00312658 File Offset: 0x00310A58
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x0031268C File Offset: 0x00310A8C
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}

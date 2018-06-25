using System;
using Steamworks;

namespace Verse.Steam
{
	internal static class WorkshopUtility
	{
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

		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}

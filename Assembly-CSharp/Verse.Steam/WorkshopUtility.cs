using Steamworks;

namespace Verse.Steam
{
	internal static class WorkshopUtility
	{
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			return (stage != 0) ? ("WorkshopInteractStage_" + stage.ToString()).Translate() : "None".Translate();
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

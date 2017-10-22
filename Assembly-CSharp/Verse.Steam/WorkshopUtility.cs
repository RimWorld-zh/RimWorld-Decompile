using Steamworks;
using System;

namespace Verse.Steam
{
	internal static class WorkshopUtility
	{
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + ((Enum)(object)stage).ToString()).Translate();
		}

		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + ((Enum)(object)status).ToString()).Translate();
		}

		public static string GetLabel(this EResult result)
		{
			return ((Enum)(object)result).ToString().Substring(9);
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	public static class PsychicDroneLevelUtility
	{
		public static string GetLabel(this PsychicDroneLevel level)
		{
			string result;
			switch (level)
			{
			case PsychicDroneLevel.None:
				result = "PsychicDroneLevel_None".Translate();
				break;
			case PsychicDroneLevel.GoodMedium:
				result = "PsychicDroneLevel_GoodMedium".Translate();
				break;
			case PsychicDroneLevel.BadLow:
				result = "PsychicDroneLevel_BadLow".Translate();
				break;
			case PsychicDroneLevel.BadMedium:
				result = "PsychicDroneLevel_BadMedium".Translate();
				break;
			case PsychicDroneLevel.BadHigh:
				result = "PsychicDroneLevel_BadHigh".Translate();
				break;
			case PsychicDroneLevel.BadExtreme:
				result = "PsychicDroneLevel_BadExtreme".Translate();
				break;
			default:
				result = "error";
				break;
			}
			return result;
		}

		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}

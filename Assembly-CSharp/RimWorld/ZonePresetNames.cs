using System;
using Verse;

namespace RimWorld
{
	public static class ZonePresetNames
	{
		public static string PresetName(this StorageSettingsPreset preset)
		{
			string result;
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				result = "DumpingStockpile".Translate();
			}
			else if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				result = "Stockpile".Translate();
			}
			else
			{
				result = "Zone".Translate();
			}
			return result;
		}
	}
}

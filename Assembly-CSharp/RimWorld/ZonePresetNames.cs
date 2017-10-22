using Verse;

namespace RimWorld
{
	public static class ZonePresetNames
	{
		public static string PresetName(this StorageSettingsPreset preset)
		{
			string result;
			switch (preset)
			{
			case StorageSettingsPreset.DumpingStockpile:
			{
				result = "DumpingStockpile".Translate();
				break;
			}
			case StorageSettingsPreset.DefaultStockpile:
			{
				result = "Stockpile".Translate();
				break;
			}
			default:
			{
				result = "Zone".Translate();
				break;
			}
			}
			return result;
		}
	}
}

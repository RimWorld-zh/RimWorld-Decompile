using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045A RID: 1114
	public static class ZonePresetNames
	{
		// Token: 0x0600138D RID: 5005 RVA: 0x000A935C File Offset: 0x000A775C
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

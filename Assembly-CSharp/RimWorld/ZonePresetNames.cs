using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045C RID: 1116
	public static class ZonePresetNames
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x000A8FF0 File Offset: 0x000A73F0
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

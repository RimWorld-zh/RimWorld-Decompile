using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045A RID: 1114
	public static class ZonePresetNames
	{
		// Token: 0x0600138E RID: 5006 RVA: 0x000A915C File Offset: 0x000A755C
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

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000458 RID: 1112
	public static class ZonePresetNames
	{
		// Token: 0x0600138A RID: 5002 RVA: 0x000A900C File Offset: 0x000A740C
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

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000455 RID: 1109
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x000A876C File Offset: 0x000A6B6C
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			return new Command_SetPlantToGrow
			{
				defaultDesc = "CommandSelectPlantToGrowDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				settable = settable
			};
		}
	}
}

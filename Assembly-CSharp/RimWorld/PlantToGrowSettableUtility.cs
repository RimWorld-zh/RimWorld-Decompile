using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000457 RID: 1111
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06001376 RID: 4982 RVA: 0x000A88BC File Offset: 0x000A6CBC
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

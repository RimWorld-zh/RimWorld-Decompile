using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000459 RID: 1113
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x0600137B RID: 4987 RVA: 0x000A8750 File Offset: 0x000A6B50
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

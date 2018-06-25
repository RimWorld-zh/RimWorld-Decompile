using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000457 RID: 1111
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06001375 RID: 4981 RVA: 0x000A8ABC File Offset: 0x000A6EBC
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

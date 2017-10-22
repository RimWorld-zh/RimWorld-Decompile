using Verse;

namespace RimWorld
{
	public static class PlantToGrowSettableUtility
	{
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			Command_SetPlantToGrow command_SetPlantToGrow = new Command_SetPlantToGrow();
			command_SetPlantToGrow.defaultDesc = "CommandSelectPlantToGrowDesc".Translate();
			command_SetPlantToGrow.hotKey = KeyBindingDefOf.Misc1;
			command_SetPlantToGrow.settable = settable;
			return command_SetPlantToGrow;
		}
	}
}

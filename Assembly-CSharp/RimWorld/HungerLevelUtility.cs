using System;
using Verse;

namespace RimWorld
{
	public static class HungerLevelUtility
	{
		public static string GetLabel(this HungerCategory hunger)
		{
			string result;
			switch (hunger)
			{
			case HungerCategory.Fed:
				result = "HungerLevel_Fed".Translate();
				break;
			case HungerCategory.Hungry:
				result = "HungerLevel_Hungry".Translate();
				break;
			case HungerCategory.UrgentlyHungry:
				result = "HungerLevel_UrgentlyHungry".Translate();
				break;
			case HungerCategory.Starving:
				result = "HungerLevel_Starving".Translate();
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}
	}
}

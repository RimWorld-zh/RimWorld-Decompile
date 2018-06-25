using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F9 RID: 1273
	public static class HungerLevelUtility
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x000CA684 File Offset: 0x000C8A84
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

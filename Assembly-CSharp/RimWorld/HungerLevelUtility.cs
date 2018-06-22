using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F7 RID: 1271
	public static class HungerLevelUtility
	{
		// Token: 0x060016DA RID: 5850 RVA: 0x000CA334 File Offset: 0x000C8734
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

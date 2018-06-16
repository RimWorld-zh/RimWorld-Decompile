using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004FB RID: 1275
	public static class HungerLevelUtility
	{
		// Token: 0x060016E2 RID: 5858 RVA: 0x000CA2E8 File Offset: 0x000C86E8
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

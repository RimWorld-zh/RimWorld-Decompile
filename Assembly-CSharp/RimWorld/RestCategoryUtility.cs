using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000504 RID: 1284
	public static class RestCategoryUtility
	{
		// Token: 0x0600170F RID: 5903 RVA: 0x000CB2CC File Offset: 0x000C96CC
		public static string GetLabel(this RestCategory fatigue)
		{
			string result;
			switch (fatigue)
			{
			case RestCategory.Rested:
				result = "HungerLevel_Rested".Translate();
				break;
			case RestCategory.Tired:
				result = "HungerLevel_Tired".Translate();
				break;
			case RestCategory.VeryTired:
				result = "HungerLevel_VeryTired".Translate();
				break;
			case RestCategory.Exhausted:
				result = "HungerLevel_Exhausted".Translate();
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}
	}
}

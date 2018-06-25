using System;
using Verse;

namespace RimWorld
{
	public static class RestCategoryUtility
	{
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

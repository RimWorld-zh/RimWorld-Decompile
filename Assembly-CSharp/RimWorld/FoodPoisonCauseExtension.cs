using System;
using Verse;

namespace RimWorld
{
	public static class FoodPoisonCauseExtension
	{
		public static string ToStringHuman(this FoodPoisonCause cause)
		{
			string result;
			switch (cause)
			{
			case FoodPoisonCause.Unknown:
				result = "Unknown".Translate();
				break;
			case FoodPoisonCause.IncompetentCook:
				result = "FoodPoisonCause_IncompetentCook".Translate();
				break;
			case FoodPoisonCause.FilthyKitchen:
				result = "FoodPoisonCause_FilthyKitchen".Translate();
				break;
			case FoodPoisonCause.Rotten:
				result = "FoodPoisonCause_Rotten".Translate();
				break;
			case FoodPoisonCause.DangerousFoodType:
				result = "FoodPoisonCause_DangerousFoodType".Translate();
				break;
			default:
				result = cause.ToString();
				break;
			}
			return result;
		}
	}
}

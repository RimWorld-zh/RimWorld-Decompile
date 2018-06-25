using System;

namespace Verse
{
	public static class DietCategoryUtility
	{
		public static string ToStringHuman(this DietCategory diet)
		{
			string result;
			switch (diet)
			{
			case DietCategory.NeverEats:
				result = "DietCategory_NeverEats".Translate();
				break;
			case DietCategory.Herbivorous:
				result = "DietCategory_Herbivorous".Translate();
				break;
			case DietCategory.Dendrovorous:
				result = "DietCategory_Dendrovorous".Translate();
				break;
			case DietCategory.Ovivorous:
				result = "DietCategory_Ovivorous".Translate();
				break;
			case DietCategory.Omnivorous:
				result = "DietCategory_Omnivorous".Translate();
				break;
			case DietCategory.Carnivorous:
				result = "DietCategory_Carnivorous".Translate();
				break;
			default:
				result = "error";
				break;
			}
			return result;
		}

		public static string ToStringHumanShort(this DietCategory diet)
		{
			string result;
			switch (diet)
			{
			case DietCategory.NeverEats:
				result = "DietCategory_NeverEats_Short".Translate();
				break;
			case DietCategory.Herbivorous:
				result = "DietCategory_Herbivorous_Short".Translate();
				break;
			case DietCategory.Dendrovorous:
				result = "DietCategory_Dendrovorous_Short".Translate();
				break;
			case DietCategory.Ovivorous:
				result = "DietCategory_Ovivorous_Short".Translate();
				break;
			case DietCategory.Omnivorous:
				result = "DietCategory_Omnivorous_Short".Translate();
				break;
			case DietCategory.Carnivorous:
				result = "DietCategory_Carnivorous_Short".Translate();
				break;
			default:
				result = "error";
				break;
			}
			return result;
		}
	}
}

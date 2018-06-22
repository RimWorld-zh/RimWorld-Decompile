using System;

namespace Verse
{
	// Token: 0x02000B1E RID: 2846
	public static class DietCategoryUtility
	{
		// Token: 0x06003EC3 RID: 16067 RVA: 0x00211180 File Offset: 0x0020F580
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

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00211220 File Offset: 0x0020F620
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

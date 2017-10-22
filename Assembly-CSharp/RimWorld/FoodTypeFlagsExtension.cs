using Verse;

namespace RimWorld
{
	public static class FoodTypeFlagsExtension
	{
		public static string ToHumanString(this FoodTypeFlags ft)
		{
			string text = "";
			if (((int)ft & 1) != 0)
			{
				text += "FoodTypeFlags_VegetableOrFruit".Translate();
			}
			if (((int)ft & 2) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Meat".Translate();
			}
			if (((int)ft & 8) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Corpse".Translate();
			}
			if (((int)ft & 16) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Seed".Translate();
			}
			if (((int)ft & 32) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_AnimalProduct".Translate();
			}
			if (((int)ft & 64) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Plant".Translate();
			}
			if (((int)ft & 128) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Tree".Translate();
			}
			if (((int)ft & 256) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Meal".Translate();
			}
			if (((int)ft & 512) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Processed".Translate();
			}
			if (((int)ft & 1024) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Liquor".Translate();
			}
			if (((int)ft & 2048) != 0)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "FoodTypeFlags_Kibble".Translate();
			}
			return text;
		}
	}
}

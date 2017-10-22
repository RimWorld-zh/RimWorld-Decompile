using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanNameGenerator
	{
		public static string GenerateCaravanName(Caravan caravan)
		{
			int num = 1;
			string result;
			while (true)
			{
				if (num <= 1000)
				{
					string text = caravan.def.label + " " + num;
					if (!CaravanNameGenerator.CaravanNameInUse(text))
					{
						result = text;
						break;
					}
					num++;
					continue;
				}
				Log.Error("Ran out of caravan names.");
				result = caravan.def.label;
				break;
			}
			return result;
		}

		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < caravans.Count)
				{
					if (caravans[num].Name == name)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}

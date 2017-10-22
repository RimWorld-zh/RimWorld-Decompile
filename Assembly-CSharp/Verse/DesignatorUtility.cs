using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public static class DesignatorUtility
	{
		public static Designator FindAllowedDesignator<T>() where T : Designator
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			GameRules rules = Current.Game.Rules;
			int num = 0;
			Designator result;
			while (true)
			{
				T val;
				if (num < allDefsListForReading.Count)
				{
					List<Designator> allResolvedDesignators = allDefsListForReading[num].AllResolvedDesignators;
					for (int i = 0; i < allResolvedDesignators.Count; i++)
					{
						if (rules.DesignatorAllowed(allResolvedDesignators[i]))
						{
							val = (T)(allResolvedDesignators[i] as T);
							if (val != null)
								goto IL_0068;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_0068:
				result = (Designator)(object)val;
				break;
			}
			return result;
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	public static class StatExtension
	{
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		public static bool StatBaseDefined(this BuildableDef def, StatDef stat)
		{
			bool result;
			if (def.statBases == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < def.statBases.Count; i++)
				{
					if (def.statBases[i].stat == stat)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}

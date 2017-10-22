using System.Collections.Generic;

namespace RimWorld
{
	public static class StatUtility
	{
		public static void SetStatValueInList(ref List<StatModifier> statList, StatDef stat, float value)
		{
			if (statList == null)
			{
				statList = new List<StatModifier>();
			}
			for (int i = 0; i < statList.Count; i++)
			{
				if (statList[i].stat == stat)
				{
					statList[i].value = value;
					return;
				}
			}
			StatModifier statModifier = new StatModifier();
			statModifier.stat = stat;
			statModifier.value = value;
			statList.Add(statModifier);
		}

		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		public static float GetStatValueFromList(this List<StatModifier> modList, StatDef stat, float defaultValue)
		{
			int i;
			if (modList != null)
			{
				for (i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
						goto IL_0022;
				}
			}
			float result = defaultValue;
			goto IL_004d;
			IL_0022:
			result = modList[i].value;
			goto IL_004d;
			IL_004d:
			return result;
		}

		public static bool StatListContains(this List<StatModifier> modList, StatDef stat)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
						goto IL_0022;
				}
			}
			bool result = false;
			goto IL_0042;
			IL_0022:
			result = true;
			goto IL_0042;
			IL_0042:
			return result;
		}
	}
}

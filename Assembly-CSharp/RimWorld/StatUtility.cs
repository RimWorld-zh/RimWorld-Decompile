using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public static class StatUtility
	{
		// Token: 0x060037E2 RID: 14306 RVA: 0x001DB61C File Offset: 0x001D9A1C
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

		// Token: 0x060037E3 RID: 14307 RVA: 0x001DB698 File Offset: 0x001D9A98
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x001DB6BC File Offset: 0x001D9ABC
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x001DB6E0 File Offset: 0x001D9AE0
		public static float GetStatValueFromList(this List<StatModifier> modList, StatDef stat, float defaultValue)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return modList[i].value;
					}
				}
			}
			return defaultValue;
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x001DB73C File Offset: 0x001D9B3C
		public static bool StatListContains(this List<StatModifier> modList, StatDef stat)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

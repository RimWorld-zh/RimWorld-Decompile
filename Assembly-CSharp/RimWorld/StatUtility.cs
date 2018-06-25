using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public static class StatUtility
	{
		// Token: 0x060037E2 RID: 14306 RVA: 0x001DB8F0 File Offset: 0x001D9CF0
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

		// Token: 0x060037E3 RID: 14307 RVA: 0x001DB96C File Offset: 0x001D9D6C
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x001DB990 File Offset: 0x001D9D90
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x001DB9B4 File Offset: 0x001D9DB4
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

		// Token: 0x060037E6 RID: 14310 RVA: 0x001DBA10 File Offset: 0x001D9E10
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

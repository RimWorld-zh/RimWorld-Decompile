using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020009BE RID: 2494
	public static class StatUtility
	{
		// Token: 0x060037DE RID: 14302 RVA: 0x001DB4DC File Offset: 0x001D98DC
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

		// Token: 0x060037DF RID: 14303 RVA: 0x001DB558 File Offset: 0x001D9958
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x001DB57C File Offset: 0x001D997C
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x001DB5A0 File Offset: 0x001D99A0
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

		// Token: 0x060037E2 RID: 14306 RVA: 0x001DB5FC File Offset: 0x001D99FC
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

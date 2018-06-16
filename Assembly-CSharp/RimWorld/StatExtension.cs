using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public static class StatExtension
	{
		// Token: 0x06003741 RID: 14145 RVA: 0x001D86D8 File Offset: 0x001D6AD8
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x001D86FC File Offset: 0x001D6AFC
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x001D8720 File Offset: 0x001D6B20
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

		// Token: 0x06003744 RID: 14148 RVA: 0x001D8784 File Offset: 0x001D6B84
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}

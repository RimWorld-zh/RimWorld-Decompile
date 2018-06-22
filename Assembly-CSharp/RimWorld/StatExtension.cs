using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public static class StatExtension
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x001D89A8 File Offset: 0x001D6DA8
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x001D89CC File Offset: 0x001D6DCC
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x001D89F0 File Offset: 0x001D6DF0
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

		// Token: 0x0600373F RID: 14143 RVA: 0x001D8A54 File Offset: 0x001D6E54
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}

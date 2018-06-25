using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099F RID: 2463
	public static class StatExtension
	{
		// Token: 0x06003740 RID: 14144 RVA: 0x001D8DBC File Offset: 0x001D71BC
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x001D8DE0 File Offset: 0x001D71E0
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x001D8E04 File Offset: 0x001D7204
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

		// Token: 0x06003743 RID: 14147 RVA: 0x001D8E68 File Offset: 0x001D7268
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}

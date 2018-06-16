using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3F RID: 2879
	public class GameConditionDef : Def
	{
		// Token: 0x06003F2D RID: 16173 RVA: 0x00213E00 File Offset: 0x00212200
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00213E40 File Offset: 0x00212240
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00213E5C File Offset: 0x0021225C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			yield break;
		}

		// Token: 0x04002967 RID: 10599
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x04002968 RID: 10600
		private List<GameConditionDef> exclusiveConditions = null;

		// Token: 0x04002969 RID: 10601
		[MustTranslate]
		public string endMessage = null;

		// Token: 0x0400296A RID: 10602
		public bool canBePermanent = false;

		// Token: 0x0400296B RID: 10603
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x0400296C RID: 10604
		public bool preventRain = false;
	}
}

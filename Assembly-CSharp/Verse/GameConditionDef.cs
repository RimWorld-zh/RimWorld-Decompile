using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3B RID: 2875
	public class GameConditionDef : Def
	{
		// Token: 0x06003F2E RID: 16174 RVA: 0x002144EC File Offset: 0x002128EC
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0021452C File Offset: 0x0021292C
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00214548 File Offset: 0x00212948
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

		// Token: 0x04002964 RID: 10596
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x04002965 RID: 10597
		private List<GameConditionDef> exclusiveConditions = null;

		// Token: 0x04002966 RID: 10598
		[MustTranslate]
		public string endMessage = null;

		// Token: 0x04002967 RID: 10599
		public bool canBePermanent = false;

		// Token: 0x04002968 RID: 10600
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04002969 RID: 10601
		public bool preventRain = false;
	}
}

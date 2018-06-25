using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3E RID: 2878
	public class GameConditionDef : Def
	{
		// Token: 0x0400296B RID: 10603
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x0400296C RID: 10604
		private List<GameConditionDef> exclusiveConditions = null;

		// Token: 0x0400296D RID: 10605
		[MustTranslate]
		public string endMessage = null;

		// Token: 0x0400296E RID: 10606
		public bool canBePermanent = false;

		// Token: 0x0400296F RID: 10607
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04002970 RID: 10608
		public bool preventRain = false;

		// Token: 0x06003F31 RID: 16177 RVA: 0x002148A8 File Offset: 0x00212CA8
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x002148E8 File Offset: 0x00212CE8
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00214904 File Offset: 0x00212D04
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
	}
}

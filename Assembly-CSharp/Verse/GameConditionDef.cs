using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3D RID: 2877
	public class GameConditionDef : Def
	{
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

		// Token: 0x06003F31 RID: 16177 RVA: 0x002145C8 File Offset: 0x002129C8
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x00214608 File Offset: 0x00212A08
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00214624 File Offset: 0x00212A24
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

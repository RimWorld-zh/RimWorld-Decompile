using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020002DE RID: 734
	public class TaleDef : Def
	{
		// Token: 0x04000782 RID: 1922
		public TaleType type = TaleType.Volatile;

		// Token: 0x04000783 RID: 1923
		public Type taleClass = null;

		// Token: 0x04000784 RID: 1924
		public bool usableForArt = true;

		// Token: 0x04000785 RID: 1925
		public bool colonistOnly = true;

		// Token: 0x04000786 RID: 1926
		public int maxPerPawn = -1;

		// Token: 0x04000787 RID: 1927
		public float ignoreChance = 0f;

		// Token: 0x04000788 RID: 1928
		public float expireDays = -1f;

		// Token: 0x04000789 RID: 1929
		public RulePack rulePack;

		// Token: 0x0400078A RID: 1930
		[NoTranslate]
		public string firstPawnSymbol = null;

		// Token: 0x0400078B RID: 1931
		[NoTranslate]
		public string secondPawnSymbol = null;

		// Token: 0x0400078C RID: 1932
		[NoTranslate]
		public string defSymbol = null;

		// Token: 0x0400078D RID: 1933
		public Type defType = typeof(ThingDef);

		// Token: 0x0400078E RID: 1934
		public float baseInterest = 0f;

		// Token: 0x0400078F RID: 1935
		public Color historyGraphColor = Color.white;

		// Token: 0x06000C17 RID: 3095 RVA: 0x0006B5E8 File Offset: 0x000699E8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.taleClass == null)
			{
				yield return this.defName + " taleClass is null.";
			}
			if (this.expireDays < 0f)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
			}
			if (this.baseInterest > 1E-06f && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
			if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
			{
				yield return "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
			}
			yield break;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0006B614 File Offset: 0x00069A14
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}
	}
}

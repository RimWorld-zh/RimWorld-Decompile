using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleDef : Def
	{
		public TaleType type = TaleType.Volatile;

		public Type taleClass = null;

		public bool usableForArt = true;

		public bool colonistOnly = true;

		public int maxPerPawn = -1;

		public float ignoreChance = 0f;

		public float expireDays = -1f;

		public RulePack rulePack;

		[NoTranslate]
		public string firstPawnSymbol = "firstPawn";

		[NoTranslate]
		public string secondPawnSymbol = "secondPawn";

		[NoTranslate]
		public string defSymbol = "def";

		public Type defType = typeof(ThingDef);

		public float baseInterest = 0f;

		public Color historyGraphColor = Color.white;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.taleClass == null)
			{
				yield return base.defName + " taleClass is null.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.expireDays < 0.0)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!(this.baseInterest > 9.9999999747524271E-07))
				yield break;
			if (this.usableForArt)
				yield break;
			yield return "Non-zero baseInterest but not usable for art";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01cc:
			/*Error near IL_01cd: Unexpected return in MoveNext()*/;
		}

		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}
	}
}

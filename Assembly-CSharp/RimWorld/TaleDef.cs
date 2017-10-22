using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleDef : Def
	{
		public TaleType type;

		public Type taleClass;

		public bool usableForArt = true;

		public bool colonistOnly = true;

		public int maxPerPawn = -1;

		public float ignoreChance;

		public float expireDays = -1f;

		public RulePack rulePack;

		[NoTranslate]
		public string firstPawnSymbol = "firstPawn";

		[NoTranslate]
		public string secondPawnSymbol = "secondPawn";

		[NoTranslate]
		public string defSymbol = "def";

		public float baseInterest;

		public Color historyGraphColor = Color.white;

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.taleClass == null)
			{
				yield return base.defName + " taleClass is null.";
			}
			if (this.expireDays < 0.0)
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
			if (this.baseInterest > 9.9999999747524271E-07 && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
		}

		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}
	}
}

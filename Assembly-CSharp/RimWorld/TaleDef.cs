using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			TaleDef.<ConfigErrors>c__Iterator99 <ConfigErrors>c__Iterator = new TaleDef.<ConfigErrors>c__Iterator99();
			<ConfigErrors>c__Iterator.<>f__this = this;
			TaleDef.<ConfigErrors>c__Iterator99 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}
	}
}

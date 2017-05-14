using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.Grammar
{
	public static class GrammarUtility
	{
		[DebuggerHidden]
		public static IEnumerable<Rule> RulesForPawn(string prefix, Name name, PawnKindDef kind, Gender gender, Faction faction = null)
		{
			GrammarUtility.<RulesForPawn>c__Iterator1EA <RulesForPawn>c__Iterator1EA = new GrammarUtility.<RulesForPawn>c__Iterator1EA();
			<RulesForPawn>c__Iterator1EA.name = name;
			<RulesForPawn>c__Iterator1EA.kind = kind;
			<RulesForPawn>c__Iterator1EA.prefix = prefix;
			<RulesForPawn>c__Iterator1EA.faction = faction;
			<RulesForPawn>c__Iterator1EA.gender = gender;
			<RulesForPawn>c__Iterator1EA.<$>name = name;
			<RulesForPawn>c__Iterator1EA.<$>kind = kind;
			<RulesForPawn>c__Iterator1EA.<$>prefix = prefix;
			<RulesForPawn>c__Iterator1EA.<$>faction = faction;
			<RulesForPawn>c__Iterator1EA.<$>gender = gender;
			GrammarUtility.<RulesForPawn>c__Iterator1EA expr_4F = <RulesForPawn>c__Iterator1EA;
			expr_4F.$PC = -2;
			return expr_4F;
		}
	}
}

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
			GrammarUtility.<RulesForPawn>c__Iterator1E4 <RulesForPawn>c__Iterator1E = new GrammarUtility.<RulesForPawn>c__Iterator1E4();
			<RulesForPawn>c__Iterator1E.name = name;
			<RulesForPawn>c__Iterator1E.kind = kind;
			<RulesForPawn>c__Iterator1E.prefix = prefix;
			<RulesForPawn>c__Iterator1E.faction = faction;
			<RulesForPawn>c__Iterator1E.gender = gender;
			<RulesForPawn>c__Iterator1E.<$>name = name;
			<RulesForPawn>c__Iterator1E.<$>kind = kind;
			<RulesForPawn>c__Iterator1E.<$>prefix = prefix;
			<RulesForPawn>c__Iterator1E.<$>faction = faction;
			<RulesForPawn>c__Iterator1E.<$>gender = gender;
			GrammarUtility.<RulesForPawn>c__Iterator1E4 expr_4F = <RulesForPawn>c__Iterator1E;
			expr_4F.$PC = -2;
			return expr_4F;
		}
	}
}

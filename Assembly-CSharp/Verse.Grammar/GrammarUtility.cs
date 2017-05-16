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
			GrammarUtility.<RulesForPawn>c__Iterator1EC <RulesForPawn>c__Iterator1EC = new GrammarUtility.<RulesForPawn>c__Iterator1EC();
			<RulesForPawn>c__Iterator1EC.name = name;
			<RulesForPawn>c__Iterator1EC.kind = kind;
			<RulesForPawn>c__Iterator1EC.prefix = prefix;
			<RulesForPawn>c__Iterator1EC.faction = faction;
			<RulesForPawn>c__Iterator1EC.gender = gender;
			<RulesForPawn>c__Iterator1EC.<$>name = name;
			<RulesForPawn>c__Iterator1EC.<$>kind = kind;
			<RulesForPawn>c__Iterator1EC.<$>prefix = prefix;
			<RulesForPawn>c__Iterator1EC.<$>faction = faction;
			<RulesForPawn>c__Iterator1EC.<$>gender = gender;
			GrammarUtility.<RulesForPawn>c__Iterator1EC expr_4F = <RulesForPawn>c__Iterator1EC;
			expr_4F.$PC = -2;
			return expr_4F;
		}
	}
}

using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	public static class GrammarUtility
	{
		public static IEnumerable<Rule> RulesForPawn(string prefix, Pawn pawn, Dictionary<string, string> constants = null)
		{
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", prefix), 16015097);
				return Enumerable.Empty<Rule>();
			}
			if (pawn.RaceProps.Humanlike)
			{
				return GrammarUtility.RulesForPawn(prefix, pawn.Name, pawn.kindDef, pawn.gender, pawn.Faction, constants);
			}
			return GrammarUtility.RulesForPawn(prefix, null, pawn.kindDef, pawn.gender, pawn.Faction, constants);
		}

		public static IEnumerable<Rule> RulesForPawn(string prefix, Name name, PawnKindDef kind, Gender gender, Faction faction, Dictionary<string, string> constants = null)
		{
			string nameFull = (name == null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label) : name.ToStringFull;
			yield return (Rule)new Rule_String(prefix + "_nameFull", nameFull);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null def", prefix), 79641686);
				yield break;
			}
			yield return (Rule)new Rule_String(prefix + "_label", def.label);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

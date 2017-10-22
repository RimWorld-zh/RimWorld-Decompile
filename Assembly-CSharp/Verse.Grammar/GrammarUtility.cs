using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	public static class GrammarUtility
	{
		public static IEnumerable<Rule> RulesForPawn(string prefix, Pawn pawn)
		{
			IEnumerable<Rule> result;
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", prefix), 16015097);
				result = Enumerable.Empty<Rule>();
			}
			else
			{
				result = ((!pawn.RaceProps.Humanlike) ? GrammarUtility.RulesForPawn(prefix, null, pawn.kindDef, pawn.gender, pawn.Faction) : GrammarUtility.RulesForPawn(prefix, pawn.Name, pawn.kindDef, pawn.gender, pawn.Faction));
			}
			return result;
		}

		public static IEnumerable<Rule> RulesForPawn(string prefix, Name name, PawnKindDef kind, Gender gender, Faction faction = null)
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

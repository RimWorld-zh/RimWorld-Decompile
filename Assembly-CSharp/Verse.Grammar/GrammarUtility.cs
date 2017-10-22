using RimWorld;
using System.Collections.Generic;

namespace Verse.Grammar
{
	public static class GrammarUtility
	{
		public static IEnumerable<Rule> RulesForPawn(string prefix, Name name, PawnKindDef kind, Gender gender, Faction faction = null)
		{
			string nameFull = (name == null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label) : name.ToStringFull;
			yield return (Rule)new Rule_String(prefix + "_nameFull", nameFull);
			string nameShort = (name == null) ? kind.label : name.ToStringShort;
			yield return (Rule)new Rule_String(prefix + "_nameShort", nameShort);
			string nameShortIndef = (name == null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label) : name.ToStringShort;
			yield return (Rule)new Rule_String(prefix + "_nameShortIndef", nameShortIndef);
			yield return (Rule)new Rule_String(prefix + "_nameShortIndefinite", nameShortIndef);
			string nameShortDef = (name == null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(kind.label) : name.ToStringShort;
			yield return (Rule)new Rule_String(prefix + "_nameShortDef", nameShortDef);
			yield return (Rule)new Rule_String(prefix + "_nameShortDefinite", nameShortDef);
			if (faction != null)
			{
				yield return (Rule)new Rule_String(prefix + "_factionName", faction.Name);
			}
			yield return (Rule)new Rule_String(prefix + "_pronoun", gender.GetPronoun());
			yield return (Rule)new Rule_String(prefix + "_possessive", gender.GetPossessive());
			yield return (Rule)new Rule_String(prefix + "_objective", gender.GetObjective());
		}
	}
}

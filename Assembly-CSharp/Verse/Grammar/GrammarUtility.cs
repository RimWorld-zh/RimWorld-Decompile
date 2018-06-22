using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BE3 RID: 3043
	public static class GrammarUtility
	{
		// Token: 0x06004278 RID: 17016 RVA: 0x0022FB58 File Offset: 0x0022DF58
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null)
		{
			IEnumerable<Rule> result;
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", pawnSymbol), 16015097, false);
				result = Enumerable.Empty<Rule>();
			}
			else
			{
				result = GrammarUtility.RulesForPawn(pawnSymbol, pawn.Name, (pawn.story == null) ? null : pawn.story.Title, pawn.kindDef, pawn.gender, pawn.Faction, constants);
			}
			return result;
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0022FBD0 File Offset: 0x0022DFD0
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, Dictionary<string, string> constants = null)
		{
			string nameFull;
			if (name != null)
			{
				nameFull = name.ToStringFull;
			}
			else
			{
				nameFull = Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label);
			}
			yield return new Rule_String(pawnSymbol + "_nameFull", nameFull);
			string nameShort;
			if (name != null)
			{
				nameShort = name.ToStringShort;
			}
			else
			{
				nameShort = kind.label;
			}
			yield return new Rule_String(pawnSymbol + "_label", nameShort);
			string nameShortDef;
			if (name != null)
			{
				nameShortDef = name.ToStringShort;
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(kind.label);
			}
			yield return new Rule_String(pawnSymbol + "_definite", nameShortDef);
			yield return new Rule_String(pawnSymbol + "_nameDef", nameShortDef);
			string nameShortIndef;
			if (name != null)
			{
				nameShortIndef = name.ToStringShort;
			}
			else
			{
				nameShortIndef = Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label);
			}
			yield return new Rule_String(pawnSymbol + "_indefinite", nameShortIndef);
			yield return new Rule_String(pawnSymbol + "_nameIndef", nameShortIndef);
			yield return new Rule_String(pawnSymbol + "_pronoun", gender.GetPronoun());
			yield return new Rule_String(pawnSymbol + "_possessive", gender.GetPossessive());
			yield return new Rule_String(pawnSymbol + "_objective", gender.GetObjective());
			if (faction != null)
			{
				yield return new Rule_String(pawnSymbol + "_factionName", faction.Name);
			}
			if (title != null)
			{
				yield return new Rule_String(pawnSymbol + "_title", title);
			}
			if (constants != null && kind != null)
			{
				constants[pawnSymbol + "_flesh"] = kind.race.race.FleshType.defName;
			}
			yield break;
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0022FC28 File Offset: 0x0022E028
		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null def", prefix), 79641686, false);
				yield break;
			}
			yield return new Rule_String(prefix + "_label", def.label);
			yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label));
			yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label));
			yield return new Rule_String(prefix + "_possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0022FC5C File Offset: 0x0022E05C
		public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			if (part == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null body part", prefix), 394876778, false);
				yield break;
			}
			yield return new Rule_String(prefix + "_label", part.Label);
			yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label));
			yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label));
			yield return new Rule_String(prefix + "_possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0022FC90 File Offset: 0x0022E090
		public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
		{
			foreach (Rule rule in GrammarUtility.RulesForDef(prefix, def))
			{
				yield return rule;
			}
			string noun = def.labelNoun;
			if (noun.NullOrEmpty())
			{
				noun = def.label;
			}
			yield return new Rule_String(prefix + "_labelNoun", noun);
			string pretty = def.PrettyTextForPart(part);
			if (!pretty.NullOrEmpty())
			{
				yield return new Rule_String(prefix + "_labelNounPretty", pretty);
			}
			yield break;
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0022FCC8 File Offset: 0x0022E0C8
		public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction)
		{
			if (faction == null)
			{
				yield return new Rule_String(prefix + "_name", "FactionUnaffiliated".Translate());
				yield break;
			}
			yield return new Rule_String(prefix + "_name", faction.Name);
			yield break;
		}
	}
}

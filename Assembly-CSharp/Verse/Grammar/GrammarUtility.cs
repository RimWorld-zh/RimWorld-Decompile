using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BE5 RID: 3045
	public static class GrammarUtility
	{
		// Token: 0x0600427B RID: 17019 RVA: 0x0022FC34 File Offset: 0x0022E034
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

		// Token: 0x0600427C RID: 17020 RVA: 0x0022FCAC File Offset: 0x0022E0AC
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

		// Token: 0x0600427D RID: 17021 RVA: 0x0022FD04 File Offset: 0x0022E104
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

		// Token: 0x0600427E RID: 17022 RVA: 0x0022FD38 File Offset: 0x0022E138
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

		// Token: 0x0600427F RID: 17023 RVA: 0x0022FD6C File Offset: 0x0022E16C
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

		// Token: 0x06004280 RID: 17024 RVA: 0x0022FDA4 File Offset: 0x0022E1A4
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.Grammar
{
	public static class GrammarUtility
	{
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null)
		{
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", pawnSymbol), 16015097, false);
				return Enumerable.Empty<Rule>();
			}
			return GrammarUtility.RulesForPawn(pawnSymbol, pawn.Name, (pawn.story == null) ? null : pawn.story.Title, pawn.kindDef, pawn.gender, pawn.Faction, constants);
		}

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
			if (kind != null)
			{
				yield return new Rule_String(pawnSymbol + "_kind", GenLabel.BestKindLabel(kind, gender, false, -1));
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

		[CompilerGenerated]
		private sealed class <RulesForPawn>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal Name name;

			internal string <nameFull>__1;

			internal PawnKindDef kind;

			internal string pawnSymbol;

			internal string <nameShort>__2;

			internal string <nameShortDef>__3;

			internal string <nameShortIndef>__4;

			internal Gender gender;

			internal Faction faction;

			internal string title;

			internal Dictionary<string, string> constants;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForPawn>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (name != null)
					{
						nameFull = name.ToStringFull;
					}
					else
					{
						nameFull = Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label);
					}
					this.$current = new Rule_String(pawnSymbol + "_nameFull", nameFull);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (name != null)
					{
						nameShort = name.ToStringShort;
					}
					else
					{
						nameShort = kind.label;
					}
					this.$current = new Rule_String(pawnSymbol + "_label", nameShort);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					if (name != null)
					{
						nameShortDef = name.ToStringShort;
					}
					else
					{
						nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(kind.label);
					}
					this.$current = new Rule_String(pawnSymbol + "_definite", nameShortDef);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = new Rule_String(pawnSymbol + "_nameDef", nameShortDef);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					if (name != null)
					{
						nameShortIndef = name.ToStringShort;
					}
					else
					{
						nameShortIndef = Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.label);
					}
					this.$current = new Rule_String(pawnSymbol + "_indefinite", nameShortIndef);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = new Rule_String(pawnSymbol + "_nameIndef", nameShortIndef);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = new Rule_String(pawnSymbol + "_pronoun", gender.GetPronoun());
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = new Rule_String(pawnSymbol + "_possessive", gender.GetPossessive());
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = new Rule_String(pawnSymbol + "_objective", gender.GetObjective());
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					if (faction != null)
					{
						this.$current = new Rule_String(pawnSymbol + "_factionName", faction.Name);
						if (!this.$disposing)
						{
							this.$PC = 10;
						}
						return true;
					}
					break;
				case 10u:
					break;
				case 11u:
					goto IL_3B4;
				case 12u:
					goto IL_3F5;
				default:
					return false;
				}
				if (kind != null)
				{
					this.$current = new Rule_String(pawnSymbol + "_kind", GenLabel.BestKindLabel(kind, gender, false, -1));
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				}
				IL_3B4:
				if (title != null)
				{
					this.$current = new Rule_String(pawnSymbol + "_title", title);
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				}
				IL_3F5:
				if (constants != null && kind != null)
				{
					constants[pawnSymbol + "_flesh"] = kind.race.race.FleshType.defName;
				}
				this.$PC = -1;
				return false;
			}

			Rule IEnumerator<Rule>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GrammarUtility.<RulesForPawn>c__Iterator0 <RulesForPawn>c__Iterator = new GrammarUtility.<RulesForPawn>c__Iterator0();
				<RulesForPawn>c__Iterator.name = name;
				<RulesForPawn>c__Iterator.kind = kind;
				<RulesForPawn>c__Iterator.pawnSymbol = pawnSymbol;
				<RulesForPawn>c__Iterator.gender = gender;
				<RulesForPawn>c__Iterator.faction = faction;
				<RulesForPawn>c__Iterator.title = title;
				<RulesForPawn>c__Iterator.constants = constants;
				return <RulesForPawn>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RulesForDef>c__Iterator1 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal Def def;

			internal string prefix;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForDef>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (def != null)
					{
						this.$current = new Rule_String(prefix + "_label", def.label);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					Log.ErrorOnce(string.Format("Tried to insert rule {0} for null def", prefix), 79641686, false);
					break;
				case 1u:
					this.$current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label));
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label));
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = new Rule_String(prefix + "_possessive", "Proits".Translate());
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Rule IEnumerator<Rule>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GrammarUtility.<RulesForDef>c__Iterator1 <RulesForDef>c__Iterator = new GrammarUtility.<RulesForDef>c__Iterator1();
				<RulesForDef>c__Iterator.def = def;
				<RulesForDef>c__Iterator.prefix = prefix;
				return <RulesForDef>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RulesForBodyPartRecord>c__Iterator2 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal BodyPartRecord part;

			internal string prefix;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForBodyPartRecord>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (part != null)
					{
						this.$current = new Rule_String(prefix + "_label", part.Label);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					Log.ErrorOnce(string.Format("Tried to insert rule {0} for null body part", prefix), 394876778, false);
					break;
				case 1u:
					this.$current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label));
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label));
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = new Rule_String(prefix + "_possessive", "Proits".Translate());
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Rule IEnumerator<Rule>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GrammarUtility.<RulesForBodyPartRecord>c__Iterator2 <RulesForBodyPartRecord>c__Iterator = new GrammarUtility.<RulesForBodyPartRecord>c__Iterator2();
				<RulesForBodyPartRecord>c__Iterator.part = part;
				<RulesForBodyPartRecord>c__Iterator.prefix = prefix;
				return <RulesForBodyPartRecord>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RulesForHediffDef>c__Iterator3 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal string prefix;

			internal HediffDef def;

			internal IEnumerator<Rule> $locvar0;

			internal Rule <rule>__1;

			internal string <noun>__0;

			internal BodyPartRecord part;

			internal string <pretty>__0;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForHediffDef>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = GrammarUtility.RulesForDef(prefix, def).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					pretty = def.PrettyTextForPart(part);
					if (!pretty.NullOrEmpty())
					{
						this.$current = new Rule_String(prefix + "_labelNounPretty", pretty);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_181;
				case 3u:
					goto IL_181;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						rule = enumerator.Current;
						this.$current = rule;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				noun = def.labelNoun;
				if (noun.NullOrEmpty())
				{
					noun = def.label;
				}
				this.$current = new Rule_String(prefix + "_labelNoun", noun);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_181:
				this.$PC = -1;
				return false;
			}

			Rule IEnumerator<Rule>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GrammarUtility.<RulesForHediffDef>c__Iterator3 <RulesForHediffDef>c__Iterator = new GrammarUtility.<RulesForHediffDef>c__Iterator3();
				<RulesForHediffDef>c__Iterator.prefix = prefix;
				<RulesForHediffDef>c__Iterator.def = def;
				<RulesForHediffDef>c__Iterator.part = part;
				return <RulesForHediffDef>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RulesForFaction>c__Iterator4 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal Faction faction;

			internal string prefix;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForFaction>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (faction == null)
					{
						this.$current = new Rule_String(prefix + "_name", "FactionUnaffiliated".Translate());
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
					}
					else
					{
						this.$current = new Rule_String(prefix + "_name", faction.Name);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Rule IEnumerator<Rule>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GrammarUtility.<RulesForFaction>c__Iterator4 <RulesForFaction>c__Iterator = new GrammarUtility.<RulesForFaction>c__Iterator4();
				<RulesForFaction>c__Iterator.faction = faction;
				<RulesForFaction>c__Iterator.prefix = prefix;
				return <RulesForFaction>c__Iterator;
			}
		}
	}
}

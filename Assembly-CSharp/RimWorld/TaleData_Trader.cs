using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleData_Trader : TaleData
	{
		public string name;

		public int pawnID = -1;

		public Gender gender = Gender.Male;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		public TaleData_Trader()
		{
		}

		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		public override IEnumerable<Rule> GetRules(string prefix)
		{
			string nameFull;
			if (this.IsPawn)
			{
				nameFull = this.name;
			}
			else
			{
				nameFull = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_nameFull", nameFull);
			string nameShortIndefinite;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_indefinite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameIndef", nameShortIndefinite);
			string nameShortDefinite;
			if (this.IsPawn)
			{
				nameShortDefinite = this.name;
			}
			else
			{
				nameShortDefinite = Find.ActiveLanguageWorker.WithDefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_definite", nameShortDefinite);
			yield return new Rule_String(prefix + "_nameDef", nameShortDefinite);
			yield return new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
			yield return new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
			yield break;
		}

		public static TaleData_Trader GenerateFrom(ITrader trader)
		{
			TaleData_Trader taleData_Trader = new TaleData_Trader();
			taleData_Trader.name = trader.TraderName;
			Pawn pawn = trader as Pawn;
			if (pawn != null)
			{
				taleData_Trader.pawnID = pawn.thingIDNumber;
				taleData_Trader.gender = pawn.gender;
			}
			return taleData_Trader;
		}

		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}

		[CompilerGenerated]
		private static bool <GenerateRandom>m__0(PawnKindDef d)
		{
			return d.trader;
		}

		[CompilerGenerated]
		private sealed class <GetRules>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal string <nameFull>__1;

			internal string prefix;

			internal string <nameShortIndefinite>__2;

			internal string <nameShortDefinite>__3;

			internal TaleData_Trader $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRules>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.IsPawn)
					{
						nameFull = this.name;
					}
					else
					{
						nameFull = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
					}
					this.$current = new Rule_String(prefix + "_nameFull", nameFull);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (base.IsPawn)
					{
						nameShortIndefinite = this.name;
					}
					else
					{
						nameShortIndefinite = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
					}
					this.$current = new Rule_String(prefix + "_indefinite", nameShortIndefinite);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new Rule_String(prefix + "_nameIndef", nameShortIndefinite);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					if (base.IsPawn)
					{
						nameShortDefinite = this.name;
					}
					else
					{
						nameShortDefinite = Find.ActiveLanguageWorker.WithDefiniteArticle(this.name);
					}
					this.$current = new Rule_String(prefix + "_definite", nameShortDefinite);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = new Rule_String(prefix + "_nameDef", nameShortDefinite);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
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
				TaleData_Trader.<GetRules>c__Iterator0 <GetRules>c__Iterator = new TaleData_Trader.<GetRules>c__Iterator0();
				<GetRules>c__Iterator.$this = this;
				<GetRules>c__Iterator.prefix = prefix;
				return <GetRules>c__Iterator;
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_DoublePawn : Tale
	{
		public TaleData_Pawn firstPawnData;

		public TaleData_Pawn secondPawnData;

		public Tale_DoublePawn()
		{
		}

		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		public override string ShortSummary
		{
			get
			{
				string text = this.def.LabelCap + ": " + this.firstPawnData.name;
				if (this.secondPawnData != null)
				{
					text = text + ", " + this.secondPawnData.name;
				}
				return text;
			}
		}

		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", new object[0]);
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", new object[0]);
		}

		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set", false);
			}
			foreach (Rule r in this.firstPawnData.GetRules("ANYPAWN"))
			{
				yield return r;
			}
			foreach (Rule r2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol))
			{
				yield return r2;
			}
			if (this.secondPawnData != null)
			{
				foreach (Rule r3 in this.firstPawnData.GetRules("ANYPAWN"))
				{
					yield return r3;
				}
				foreach (Rule r4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol))
				{
					yield return r4;
				}
			}
			yield break;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}

		[CompilerGenerated]
		private sealed class <SpecialTextGenerationRules>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal IEnumerator<Rule> $locvar0;

			internal Rule <r>__1;

			internal IEnumerator<Rule> $locvar1;

			internal Rule <r>__2;

			internal IEnumerator<Rule> $locvar2;

			internal Rule <r>__3;

			internal IEnumerator<Rule> $locvar3;

			internal Rule <r>__4;

			internal Tale_DoublePawn $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialTextGenerationRules>c__Iterator0()
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
					if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
					{
						Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set", false);
					}
					enumerator = this.firstPawnData.GetRules("ANYPAWN").GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_148;
				case 3u:
					goto IL_1F3;
				case 4u:
					goto IL_298;
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
						r = enumerator.Current;
						this.$current = r;
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
				enumerator2 = this.firstPawnData.GetRules(this.def.firstPawnSymbol).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_148:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						r2 = enumerator2.Current;
						this.$current = r2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				if (this.secondPawnData == null)
				{
					goto IL_30F;
				}
				enumerator3 = this.firstPawnData.GetRules("ANYPAWN").GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_1F3:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						r3 = enumerator3.Current;
						this.$current = r3;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				enumerator4 = this.secondPawnData.GetRules(this.def.secondPawnSymbol).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_298:
					switch (num)
					{
					}
					if (enumerator4.MoveNext())
					{
						r4 = enumerator4.Current;
						this.$current = r4;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
						}
					}
				}
				IL_30F:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				case 4u:
					try
					{
					}
					finally
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				Tale_DoublePawn.<SpecialTextGenerationRules>c__Iterator0 <SpecialTextGenerationRules>c__Iterator = new Tale_DoublePawn.<SpecialTextGenerationRules>c__Iterator0();
				<SpecialTextGenerationRules>c__Iterator.$this = this;
				return <SpecialTextGenerationRules>c__Iterator;
			}
		}
	}
}

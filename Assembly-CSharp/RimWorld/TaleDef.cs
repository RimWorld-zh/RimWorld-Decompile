using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleDef : Def
	{
		public TaleType type = TaleType.Volatile;

		public Type taleClass = null;

		public bool usableForArt = true;

		public bool colonistOnly = true;

		public int maxPerPawn = -1;

		public float ignoreChance = 0f;

		public float expireDays = -1f;

		public RulePack rulePack;

		[NoTranslate]
		public string firstPawnSymbol = null;

		[NoTranslate]
		public string secondPawnSymbol = null;

		[NoTranslate]
		public string defSymbol = null;

		public Type defType = typeof(ThingDef);

		public float baseInterest = 0f;

		public Color historyGraphColor = Color.white;

		public TaleDef()
		{
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.taleClass == null)
			{
				yield return this.defName + " taleClass is null.";
			}
			if (this.expireDays < 0f)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
			}
			if (this.baseInterest > 1E-06f && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
			if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
			{
				yield return "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
			}
			yield break;
		}

		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal TaleDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_107;
				case 3u:
					goto IL_14D;
				case 4u:
					goto IL_183;
				case 5u:
					goto IL_1C7;
				case 6u:
					goto IL_21A;
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
						err = enumerator.Current;
						this.$current = err;
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
				if (this.taleClass == null)
				{
					this.$current = this.defName + " taleClass is null.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_107:
				if (this.expireDays < 0f)
				{
					if (this.type == TaleType.Expirable)
					{
						this.$current = "Expirable tale type is used but expireDays<0";
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
				}
				else if (this.type != TaleType.Expirable)
				{
					this.$current = "Non expirable tale type is used but expireDays>=0";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_14D:
				IL_183:
				if (this.baseInterest > 1E-06f && !this.usableForArt)
				{
					this.$current = "Non-zero baseInterest but not usable for art";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1C7:
				if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
				{
					this.$current = "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_21A:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TaleDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new TaleDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}

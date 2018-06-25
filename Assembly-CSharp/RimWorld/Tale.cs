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
	public class Tale : IExposable, ILoadReferenceable
	{
		public TaleDef def;

		public int id;

		private int uses = 0;

		public int date = -1;

		public TaleData_Surroundings surroundings;

		public Tale()
		{
		}

		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		public float InterestLevel
		{
			get
			{
				float num = this.def.baseInterest;
				num /= (float)(1 + this.uses * 3);
				float a = 0f;
				TaleType type = this.def.type;
				if (type != TaleType.Volatile)
				{
					if (type != TaleType.PermanentHistorical)
					{
						if (type == TaleType.Expirable)
						{
							a = this.def.expireDays;
						}
					}
					else
					{
						a = 50f;
					}
				}
				else
				{
					a = 50f;
				}
				float value = (float)(this.AgeTicks / 60000);
				num *= Mathf.InverseLerp(a, 0f, value);
				if (num < 0.01f)
				{
					num = 0.01f;
				}
				return num;
			}
		}

		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		public virtual string ShortSummary
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.", false);
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", new object[0]);
		}

		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + this + " but uses count is 0.", false);
			}
			else
			{
				this.uses--;
			}
		}

		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		public IEnumerable<Rule> GetTextGenerationRules()
		{
			Vector2 location = Vector2.zero;
			if (this.surroundings != null && this.surroundings.tile >= 0)
			{
				location = Find.WorldGrid.LongLatOf(this.surroundings.tile);
			}
			yield return new Rule_String("DATE", GenDate.DateFullStringAt((long)this.date, location));
			if (this.surroundings != null)
			{
				foreach (Rule r in this.surroundings.GetRules())
				{
					yield return r;
				}
			}
			foreach (Rule r2 in this.SpecialTextGenerationRules())
			{
				yield return r2;
			}
			yield break;
		}

		protected virtual IEnumerable<Rule> SpecialTextGenerationRules()
		{
			yield break;
		}

		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		public override int GetHashCode()
		{
			return this.id;
		}

		public override string ToString()
		{
			string str = string.Concat(new object[]
			{
				"(#",
				this.id,
				": ",
				this.ShortSummary,
				"(age=",
				((float)this.AgeTicks / 60000f).ToString("F2"),
				" interest=",
				this.InterestLevel
			});
			if (this.Unused && this.def.type == TaleType.Expirable)
			{
				str = str + ", expireDays=" + this.def.expireDays.ToString("F2");
			}
			return str + ")";
		}

		[CompilerGenerated]
		private sealed class <GetTextGenerationIncludes>c__Iterator0 : IEnumerable, IEnumerable<RulePack>, IEnumerator, IDisposable, IEnumerator<RulePack>
		{
			internal Tale $this;

			internal RulePack $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTextGenerationIncludes>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.def.rulePack != null)
					{
						this.$current = this.def.rulePack;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			RulePack IEnumerator<RulePack>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.RulePack>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<RulePack> IEnumerable<RulePack>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Tale.<GetTextGenerationIncludes>c__Iterator0 <GetTextGenerationIncludes>c__Iterator = new Tale.<GetTextGenerationIncludes>c__Iterator0();
				<GetTextGenerationIncludes>c__Iterator.$this = this;
				return <GetTextGenerationIncludes>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTextGenerationRules>c__Iterator1 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal Vector2 <location>__0;

			internal IEnumerator<Rule> $locvar0;

			internal Rule <r>__1;

			internal IEnumerator<Rule> $locvar1;

			internal Rule <r>__2;

			internal Tale $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTextGenerationRules>c__Iterator1()
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
					location = Vector2.zero;
					if (this.surroundings != null && this.surroundings.tile >= 0)
					{
						location = Find.WorldGrid.LongLatOf(this.surroundings.tile);
					}
					this.$current = new Rule_String("DATE", GenDate.DateFullStringAt((long)this.date, location));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (this.surroundings == null)
					{
						goto IL_15F;
					}
					enumerator = this.surroundings.GetRules().GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				case 3u:
					Block_7:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							r2 = enumerator2.Current;
							this.$current = r2;
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
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_15F:
				enumerator2 = this.SpecialTextGenerationRules().GetEnumerator();
				num = 4294967293u;
				goto Block_7;
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
				case 2u:
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
				case 3u:
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
				Tale.<GetTextGenerationRules>c__Iterator1 <GetTextGenerationRules>c__Iterator = new Tale.<GetTextGenerationRules>c__Iterator1();
				<GetTextGenerationRules>c__Iterator.$this = this;
				return <GetTextGenerationRules>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialTextGenerationRules>c__Iterator2 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialTextGenerationRules>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
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
				return new Tale.<SpecialTextGenerationRules>c__Iterator2();
			}
		}
	}
}

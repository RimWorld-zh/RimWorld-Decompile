using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using RimWorld;

namespace Verse
{
	public class Def : Editable
	{
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		[DefaultValue(null)]
		[Description("A human-readable label used to identify this in game.")]
		[MustTranslate]
		public string label = null;

		[DefaultValue(null)]
		[Description("A human-readable description given when the Def is inspected by players.")]
		[MustTranslate]
		public string description = null;

		[DefaultValue(false)]
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[MustTranslate]
		public bool ignoreConfigErrors = false;

		[DefaultValue(null)]
		[Description("Mod-specific data. Not used by core game code.")]
		public List<DefModExtension> modExtensions;

		[Unsaved]
		public ushort shortHash;

		[Unsaved]
		public ushort index = ushort.MaxValue;

		[Unsaved]
		public ModContentPack modContentPack;

		[Unsaved]
		private string cachedLabelCap = null;

		[Unsaved]
		public bool generated;

		[Unsaved]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		public const string DefaultDefName = "UnnamedDef";

		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		public Def()
		{
		}

		public string LabelCap
		{
			get
			{
				string result;
				if (this.label.NullOrEmpty())
				{
					result = null;
				}
				else
				{
					if (this.cachedLabelCap.NullOrEmpty())
					{
						this.cachedLabelCap = this.label.CapitalizeFirst();
					}
					result = this.cachedLabelCap;
				}
				return result;
			}
		}

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					foreach (string err in this.modExtensions[i].ConfigErrors())
					{
						yield return err;
					}
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			yield break;
		}

		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		public override string ToString()
		{
			return this.defName;
		}

		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		public T GetModExtension<T>() where T : DefModExtension
		{
			T result;
			if (this.modExtensions == null)
			{
				result = (T)((object)null);
			}
			else
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					if (this.modExtensions[i] is T)
					{
						return this.modExtensions[i] as T;
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Def()
		{
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator0 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator0()
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

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Def.<SpecialDisplayStats>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal int <i>__1;

			internal IEnumerator<string> $locvar0;

			internal string <err>__2;

			internal Def $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator1()
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
					if (this.defName == "UnnamedDef")
					{
						this.$current = base.GetType() + " lacks defName. Label=" + this.label;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_C9;
				case 3u:
					goto IL_117;
				case 4u:
					Block_9:
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
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					i++;
					goto IL_1E4;
				case 5u:
					IL_24A:
					if (char.IsWhiteSpace(this.description[0]))
					{
						this.$current = "description has leading whitespace";
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
					goto IL_284;
				case 6u:
					goto IL_284;
				case 7u:
					goto IL_2CF;
				default:
					return false;
				}
				if (this.defName == "null")
				{
					this.$current = "defName cannot be the string 'null'.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_C9:
				if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
				{
					this.$current = "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_117:
				if (this.modExtensions == null)
				{
					goto IL_200;
				}
				i = 0;
				IL_1E4:
				if (i < this.modExtensions.Count)
				{
					enumerator = this.modExtensions[i].ConfigErrors().GetEnumerator();
					num = 4294967293u;
					goto Block_9;
				}
				IL_200:
				if (this.description == null)
				{
					goto IL_2D0;
				}
				if (this.description == "")
				{
					this.$current = "empty description";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				goto IL_24A;
				IL_284:
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					this.$current = "description has trailing whitespace";
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_2CF:
				IL_2D0:
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
				case 4u:
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
				Def.<ConfigErrors>c__Iterator1 <ConfigErrors>c__Iterator = new Def.<ConfigErrors>c__Iterator1();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}

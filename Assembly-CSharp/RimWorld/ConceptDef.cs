using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ConceptDef : Def
	{
		public float priority = float.MaxValue;

		public bool noteTeaches = false;

		public bool needsOpportunity = false;

		public bool opportunityDecays = true;

		public ProgramState gameMode = ProgramState.Playing;

		[MustTranslate]
		private string helpText = null;

		[NoTranslate]
		public List<string> highlightTags = null;

		private static List<string> tmpParseErrors = new List<string>();

		public ConceptDef()
		{
		}

		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = this.defName.ToString();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string str in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return str;
			}
			if (this.priority > 9999999f)
			{
				yield return "priority isn't set";
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
			}
			if (this.TriggeredDirect && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			ConceptDef.tmpParseErrors.Clear();
			this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
			for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i++)
			{
				yield return "helpText error: " + ConceptDef.tmpParseErrors[i];
			}
			yield break;
		}

		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ConceptDef()
		{
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

			internal string <str>__1;

			internal int <i>__2;

			internal ConceptDef $this;

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
					goto IL_F8;
				case 3u:
					goto IL_12C;
				case 4u:
					goto IL_170;
				case 5u:
					i++;
					goto IL_1E1;
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
						str = enumerator.Current;
						this.$current = str;
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
				if (this.priority > 9999999f)
				{
					this.$current = "priority isn't set";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_F8:
				if (this.helpText.NullOrEmpty())
				{
					this.$current = "no help text";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_12C:
				if (base.TriggeredDirect && this.label.NullOrEmpty())
				{
					this.$current = "no label";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_170:
				ConceptDef.tmpParseErrors.Clear();
				this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
				i = 0;
				IL_1E1:
				if (i < ConceptDef.tmpParseErrors.Count)
				{
					this.$current = "helpText error: " + ConceptDef.tmpParseErrors[i];
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
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
				ConceptDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new ConceptDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}

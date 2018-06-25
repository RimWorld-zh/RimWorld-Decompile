using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class InstructionDef : Def
	{
		public Type instructionClass = typeof(Instruction_Basic);

		[MustTranslate]
		public string text;

		public bool startCentered = false;

		public bool tutorialModeOnly = true;

		[NoTranslate]
		public string eventTagInitiate;

		public InstructionDef eventTagInitiateSource;

		[NoTranslate]
		public List<string> eventTagsEnd;

		[NoTranslate]
		public List<string> actionTagsAllowed = null;

		[MustTranslate]
		public string rejectInputMessage = null;

		public ConceptDef concept = null;

		[NoTranslate]
		public List<string> highlightTags;

		[MustTranslate]
		public string onMapInstruction;

		public int targetCount;

		public ThingDef thingDef;

		public RecipeDef recipeDef;

		public int recipeTargetCount = 1;

		public ThingDef giveOnActivateDef;

		public int giveOnActivateCount;

		public bool endTutorial = false;

		public bool resetBuildDesignatorStuffs = false;

		private static List<string> tmpParseErrors = new List<string>();

		public InstructionDef()
		{
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
			}
			if (this.eventTagInitiate.NullOrEmpty())
			{
				yield return "no eventTagInitiate";
			}
			InstructionDef.tmpParseErrors.Clear();
			this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
			for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i++)
			{
				yield return "text error: " + InstructionDef.tmpParseErrors[i];
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static InstructionDef()
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

			internal string <e>__1;

			internal int <i>__2;

			internal InstructionDef $this;

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
					goto IL_F3;
				case 3u:
					goto IL_127;
				case 4u:
					goto IL_15B;
				case 5u:
					i++;
					goto IL_1CC;
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
						e = enumerator.Current;
						this.$current = e;
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
				if (this.instructionClass == null)
				{
					this.$current = "no instruction class";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_F3:
				if (this.text.NullOrEmpty())
				{
					this.$current = "no text";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_127:
				if (this.eventTagInitiate.NullOrEmpty())
				{
					this.$current = "no eventTagInitiate";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_15B:
				InstructionDef.tmpParseErrors.Clear();
				this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
				i = 0;
				IL_1CC:
				if (i < InstructionDef.tmpParseErrors.Count)
				{
					this.$current = "text error: " + InstructionDef.tmpParseErrors[i];
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
				InstructionDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new InstructionDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerCompProperties
	{
		[TranslationHandle]
		public Type compClass;

		public float minDaysPassed = 0f;

		public List<IncidentTargetTypeDef> allowedTargetTypes = null;

		public float minIncChancePopulationIntentFactor = 0.05f;

		public StorytellerCompProperties()
		{
		}

		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has StorytellerCompProperties with null compClass.";
			}
			yield break;
		}

		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal StorytellerDef parentDef;

			internal StorytellerCompProperties $this;

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
				switch (num)
				{
				case 0u:
					if (this.compClass == null)
					{
						this.$current = parentDef.defName + " has StorytellerCompProperties with null compClass.";
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorytellerCompProperties.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new StorytellerCompProperties.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parentDef = parentDef;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}

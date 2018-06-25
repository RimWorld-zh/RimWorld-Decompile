using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public class StandardLetter : ChoiceLetter
	{
		public StandardLetter()
		{
		}

		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.OK;
				if (this.lookTargets.IsValid())
				{
					yield return base.JumpToLocation;
				}
				yield break;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<DiaOption>, IEnumerator, IDisposable, IEnumerator<DiaOption>
		{
			internal StandardLetter $this;

			internal DiaOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = base.OK;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (this.lookTargets.IsValid())
					{
						this.$current = base.JumpToLocation;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			DiaOption IEnumerator<DiaOption>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.DiaOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StandardLetter.<>c__Iterator0 <>c__Iterator = new StandardLetter.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}

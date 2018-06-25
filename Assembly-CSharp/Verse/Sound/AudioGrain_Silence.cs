using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse.Sound
{
	public class AudioGrain_Silence : AudioGrain
	{
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);

		public AudioGrain_Silence()
		{
		}

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		[CompilerGenerated]
		private sealed class <GetResolvedGrains>c__Iterator0 : IEnumerable, IEnumerable<ResolvedGrain>, IEnumerator, IDisposable, IEnumerator<ResolvedGrain>
		{
			internal AudioGrain_Silence $this;

			internal ResolvedGrain $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetResolvedGrains>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = new ResolvedGrain_Silence(this);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			ResolvedGrain IEnumerator<ResolvedGrain>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Sound.ResolvedGrain>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ResolvedGrain> IEnumerable<ResolvedGrain>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				AudioGrain_Silence.<GetResolvedGrains>c__Iterator0 <GetResolvedGrains>c__Iterator = new AudioGrain_Silence.<GetResolvedGrains>c__Iterator0();
				<GetResolvedGrains>c__Iterator.$this = this;
				return <GetResolvedGrains>c__Iterator;
			}
		}
	}
}

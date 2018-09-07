using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioGrain_Clip : AudioGrain
	{
		[NoTranslate]
		public string clipPath = string.Empty;

		public AudioGrain_Clip()
		{
		}

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (clip != null)
			{
				yield return new ResolvedGrain_Clip(clip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath, false);
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetResolvedGrains>c__Iterator0 : IEnumerable, IEnumerable<ResolvedGrain>, IEnumerator, IDisposable, IEnumerator<ResolvedGrain>
		{
			internal AudioClip <clip>__0;

			internal AudioGrain_Clip $this;

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
					clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
					if (clip != null)
					{
						this.$current = new ResolvedGrain_Clip(clip);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath, false);
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
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
				AudioGrain_Clip.<GetResolvedGrains>c__Iterator0 <GetResolvedGrains>c__Iterator = new AudioGrain_Clip.<GetResolvedGrains>c__Iterator0();
				<GetResolvedGrains>c__Iterator.$this = this;
				return <GetResolvedGrains>c__Iterator;
			}
		}
	}
}

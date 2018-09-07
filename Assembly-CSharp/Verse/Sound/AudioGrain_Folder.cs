using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioGrain_Folder : AudioGrain
	{
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = string.Empty;

		public AudioGrain_Folder()
		{
		}

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip folderClip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(folderClip);
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetResolvedGrains>c__Iterator0 : IEnumerable, IEnumerable<ResolvedGrain>, IEnumerator, IDisposable, IEnumerator<ResolvedGrain>
		{
			internal IEnumerator<AudioClip> $locvar0;

			internal AudioClip <folderClip>__1;

			internal AudioGrain_Folder $this;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
						folderClip = enumerator.Current;
						this.$current = new ResolvedGrain_Clip(folderClip);
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
				return this.System.Collections.Generic.IEnumerable<Verse.Sound.ResolvedGrain>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ResolvedGrain> IEnumerable<ResolvedGrain>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				AudioGrain_Folder.<GetResolvedGrains>c__Iterator0 <GetResolvedGrains>c__Iterator = new AudioGrain_Folder.<GetResolvedGrains>c__Iterator0();
				<GetResolvedGrains>c__Iterator.$this = this;
				return <GetResolvedGrains>c__Iterator;
			}
		}
	}
}

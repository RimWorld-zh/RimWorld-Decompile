using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public static class GenString
	{
		private static string[] numberStrings = new string[10000];

		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		public static string ToStringCached(this int num)
		{
			string result;
			if (num < -4999)
			{
				result = num.ToString();
			}
			else if (num > 4999)
			{
				result = num.ToString();
			}
			else
			{
				result = GenString.numberStrings[num + 5000];
			}
			return result;
		}

		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <SplitBy>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string str;

			internal int chunkLength;

			internal int <i>__1;

			internal string $current;

			internal bool $disposing;

			internal int <$>chunkLength;

			internal int $PC;

			[DebuggerHidden]
			public <SplitBy>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (str.NullOrEmpty())
					{
						return false;
					}
					if (chunkLength < 1)
					{
						throw new ArgumentException();
					}
					i = 0;
					break;
				case 1u:
					i += chunkLength;
					break;
				default:
					return false;
				}
				if (i < str.Length)
				{
					if (chunkLength > str.Length - i)
					{
						chunkLength = str.Length - i;
					}
					this.$current = str.Substring(i, chunkLength);
					if (!this.$disposing)
					{
						this.$PC = 1;
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
				GenString.<SplitBy>c__Iterator0 <SplitBy>c__Iterator = new GenString.<SplitBy>c__Iterator0();
				<SplitBy>c__Iterator.str = str;
				<SplitBy>c__Iterator.chunkLength = chunkLength;
				return <SplitBy>c__Iterator;
			}
		}
	}
}

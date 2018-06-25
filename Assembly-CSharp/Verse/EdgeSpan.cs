using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public struct EdgeSpan
	{
		public IntVec3 root;

		public SpanDirection dir;

		public int length;

		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				for (int i = 0; i < this.length; i++)
				{
					if (this.dir == SpanDirection.North)
					{
						yield return new IntVec3(this.root.x, 0, this.root.z + i);
					}
					else if (this.dir == SpanDirection.East)
					{
						yield return new IntVec3(this.root.x + i, 0, this.root.z);
					}
				}
				yield break;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(root=",
				this.root,
				", dir=",
				this.dir.ToString(),
				" + length=",
				this.length,
				")"
			});
		}

		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <i>__1;

			internal EdgeSpan $this;

			internal IntVec3 $current;

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
					i = 0;
					goto IL_F6;
				case 1u:
					break;
				case 2u:
					break;
				default:
					return false;
				}
				IL_E7:
				i++;
				IL_F6:
				if (i < this.length)
				{
					if (this.dir == SpanDirection.North)
					{
						this.$current = new IntVec3(this.root.x, 0, this.root.z + i);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
					}
					else
					{
						if (this.dir != SpanDirection.East)
						{
							goto IL_E7;
						}
						this.$current = new IntVec3(this.root.x + i, 0, this.root.z);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				EdgeSpan.<>c__Iterator0 <>c__Iterator = new EdgeSpan.<>c__Iterator0();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}
	}
}

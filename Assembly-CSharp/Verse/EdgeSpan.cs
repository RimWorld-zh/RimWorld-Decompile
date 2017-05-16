using System;
using System.Collections.Generic;

namespace Verse
{
	public struct EdgeSpan
	{
		public IntVec3 root;

		public SpanDirection dir;

		public int length;

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
				EdgeSpan.<>c__Iterator204 <>c__Iterator = new EdgeSpan.<>c__Iterator204();
				<>c__Iterator.<>f__this = this;
				EdgeSpan.<>c__Iterator204 expr_13 = <>c__Iterator;
				expr_13.$PC = -2;
				return expr_13;
			}
		}

		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
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
				num += 17592186044416uL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}
	}
}

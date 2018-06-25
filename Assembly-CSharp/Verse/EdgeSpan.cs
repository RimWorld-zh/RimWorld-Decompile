using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8E RID: 3214
	public struct EdgeSpan
	{
		// Token: 0x0400300F RID: 12303
		public IntVec3 root;

		// Token: 0x04003010 RID: 12304
		public SpanDirection dir;

		// Token: 0x04003011 RID: 12305
		public int length;

		// Token: 0x06004682 RID: 18050 RVA: 0x00253C56 File Offset: 0x00252056
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x00253C70 File Offset: 0x00252070
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06004684 RID: 18052 RVA: 0x00253C90 File Offset: 0x00252090
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

		// Token: 0x06004685 RID: 18053 RVA: 0x00253CC0 File Offset: 0x002520C0
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

		// Token: 0x06004686 RID: 18054 RVA: 0x00253D30 File Offset: 0x00252130
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}
	}
}

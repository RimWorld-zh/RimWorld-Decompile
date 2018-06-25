using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8D RID: 3213
	public struct EdgeSpan
	{
		// Token: 0x04003008 RID: 12296
		public IntVec3 root;

		// Token: 0x04003009 RID: 12297
		public SpanDirection dir;

		// Token: 0x0400300A RID: 12298
		public int length;

		// Token: 0x06004682 RID: 18050 RVA: 0x00253976 File Offset: 0x00251D76
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x00253990 File Offset: 0x00251D90
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06004684 RID: 18052 RVA: 0x002539B0 File Offset: 0x00251DB0
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

		// Token: 0x06004685 RID: 18053 RVA: 0x002539E0 File Offset: 0x00251DE0
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

		// Token: 0x06004686 RID: 18054 RVA: 0x00253A50 File Offset: 0x00251E50
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

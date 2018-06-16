using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8F RID: 3215
	public struct EdgeSpan
	{
		// Token: 0x06004678 RID: 18040 RVA: 0x002524F2 File Offset: 0x002508F2
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x0025250C File Offset: 0x0025090C
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600467A RID: 18042 RVA: 0x0025252C File Offset: 0x0025092C
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

		// Token: 0x0600467B RID: 18043 RVA: 0x0025255C File Offset: 0x0025095C
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

		// Token: 0x0600467C RID: 18044 RVA: 0x002525CC File Offset: 0x002509CC
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04003000 RID: 12288
		public IntVec3 root;

		// Token: 0x04003001 RID: 12289
		public SpanDirection dir;

		// Token: 0x04003002 RID: 12290
		public int length;
	}
}

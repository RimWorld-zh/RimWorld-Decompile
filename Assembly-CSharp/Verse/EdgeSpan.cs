using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8E RID: 3214
	public struct EdgeSpan
	{
		// Token: 0x06004676 RID: 18038 RVA: 0x002524CA File Offset: 0x002508CA
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x002524E4 File Offset: 0x002508E4
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06004678 RID: 18040 RVA: 0x00252504 File Offset: 0x00250904
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

		// Token: 0x06004679 RID: 18041 RVA: 0x00252534 File Offset: 0x00250934
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

		// Token: 0x0600467A RID: 18042 RVA: 0x002525A4 File Offset: 0x002509A4
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04002FFE RID: 12286
		public IntVec3 root;

		// Token: 0x04002FFF RID: 12287
		public SpanDirection dir;

		// Token: 0x04003000 RID: 12288
		public int length;
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB3 RID: 4019
	public struct ShootLine
	{
		// Token: 0x06006143 RID: 24899 RVA: 0x00312243 File Offset: 0x00310643
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06006144 RID: 24900 RVA: 0x00312254 File Offset: 0x00310654
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06006145 RID: 24901 RVA: 0x00312270 File Offset: 0x00310670
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x0031228C File Offset: 0x0031068C
		public void ChangeDestToMissWild()
		{
			if ((double)(this.dest - this.source).LengthHorizontal < 2.5)
			{
				IntVec3 b = IntVec3.FromVector3((this.dest - this.source).ToVector3().normalized * 2f);
				this.dest += b;
			}
			this.dest = this.dest.RandomAdjacentCell8Way();
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x00312318 File Offset: 0x00310718
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x00312340 File Offset: 0x00310740
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.source,
				"->",
				this.dest,
				")"
			});
		}

		// Token: 0x04003F8F RID: 16271
		private IntVec3 source;

		// Token: 0x04003F90 RID: 16272
		private IntVec3 dest;
	}
}

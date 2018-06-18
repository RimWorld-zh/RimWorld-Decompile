using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB3 RID: 4019
	public struct ShootLine
	{
		// Token: 0x0600611A RID: 24858 RVA: 0x0031019F File Offset: 0x0030E59F
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x0600611B RID: 24859 RVA: 0x003101B0 File Offset: 0x0030E5B0
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x0600611C RID: 24860 RVA: 0x003101CC File Offset: 0x0030E5CC
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x003101E8 File Offset: 0x0030E5E8
		public void ChangeDestToMissWild()
		{
			if ((double)(this.dest - this.source).LengthHorizontal < 2.5)
			{
				IntVec3 b = IntVec3.FromVector3((this.dest - this.source).ToVector3().normalized * 2f);
				this.dest += b;
			}
			this.dest = this.dest.RandomAdjacentCell8Way();
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x00310274 File Offset: 0x0030E674
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x0031029C File Offset: 0x0030E69C
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

		// Token: 0x04003F7D RID: 16253
		private IntVec3 source;

		// Token: 0x04003F7E RID: 16254
		private IntVec3 dest;
	}
}

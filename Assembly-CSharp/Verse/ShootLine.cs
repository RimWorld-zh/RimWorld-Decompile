using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB4 RID: 4020
	public struct ShootLine
	{
		// Token: 0x0600611C RID: 24860 RVA: 0x003100C3 File Offset: 0x0030E4C3
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x0600611D RID: 24861 RVA: 0x003100D4 File Offset: 0x0030E4D4
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x0600611E RID: 24862 RVA: 0x003100F0 File Offset: 0x0030E4F0
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x0031010C File Offset: 0x0030E50C
		public void ChangeDestToMissWild()
		{
			if ((double)(this.dest - this.source).LengthHorizontal < 2.5)
			{
				IntVec3 b = IntVec3.FromVector3((this.dest - this.source).ToVector3().normalized * 2f);
				this.dest += b;
			}
			this.dest = this.dest.RandomAdjacentCell8Way();
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x00310198 File Offset: 0x0030E598
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x003101C0 File Offset: 0x0030E5C0
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

		// Token: 0x04003F7E RID: 16254
		private IntVec3 source;

		// Token: 0x04003F7F RID: 16255
		private IntVec3 dest;
	}
}

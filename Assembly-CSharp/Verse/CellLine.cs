using System;

namespace Verse
{
	// Token: 0x02000ED2 RID: 3794
	public struct CellLine
	{
		// Token: 0x04003C14 RID: 15380
		private float zIntercept;

		// Token: 0x04003C15 RID: 15381
		private float slope;

		// Token: 0x060059F6 RID: 23030 RVA: 0x002E3321 File Offset: 0x002E1721
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x002E3332 File Offset: 0x002E1732
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060059F8 RID: 23032 RVA: 0x002E3358 File Offset: 0x002E1758
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x060059F9 RID: 23033 RVA: 0x002E3374 File Offset: 0x002E1774
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x002E3390 File Offset: 0x002E1790
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 1E+08f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			float num2 = (float)a.z - (float)a.x * num;
			return new CellLine(num2, num);
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x002E3400 File Offset: 0x002E1800
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}
	}
}

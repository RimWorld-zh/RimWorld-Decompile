using System;

namespace Verse
{
	// Token: 0x02000ED3 RID: 3795
	public struct CellLine
	{
		// Token: 0x060059D5 RID: 22997 RVA: 0x002E150D File Offset: 0x002DF90D
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x002E151E File Offset: 0x002DF91E
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060059D7 RID: 22999 RVA: 0x002E1544 File Offset: 0x002DF944
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060059D8 RID: 23000 RVA: 0x002E1560 File Offset: 0x002DF960
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x002E157C File Offset: 0x002DF97C
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

		// Token: 0x060059DA RID: 23002 RVA: 0x002E15EC File Offset: 0x002DF9EC
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04003C04 RID: 15364
		private float zIntercept;

		// Token: 0x04003C05 RID: 15365
		private float slope;
	}
}

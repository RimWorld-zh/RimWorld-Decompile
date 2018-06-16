using System;

namespace Verse
{
	// Token: 0x02000ED4 RID: 3796
	public struct CellLine
	{
		// Token: 0x060059D7 RID: 22999 RVA: 0x002E1435 File Offset: 0x002DF835
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x002E1446 File Offset: 0x002DF846
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060059D9 RID: 23001 RVA: 0x002E146C File Offset: 0x002DF86C
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060059DA RID: 23002 RVA: 0x002E1488 File Offset: 0x002DF888
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x002E14A4 File Offset: 0x002DF8A4
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

		// Token: 0x060059DC RID: 23004 RVA: 0x002E1514 File Offset: 0x002DF914
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04003C05 RID: 15365
		private float zIntercept;

		// Token: 0x04003C06 RID: 15366
		private float slope;
	}
}

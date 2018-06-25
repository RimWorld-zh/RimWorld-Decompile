using System;

namespace Verse
{
	// Token: 0x02000ED5 RID: 3797
	public struct CellLine
	{
		// Token: 0x04003C1C RID: 15388
		private float zIntercept;

		// Token: 0x04003C1D RID: 15389
		private float slope;

		// Token: 0x060059F9 RID: 23033 RVA: 0x002E3661 File Offset: 0x002E1A61
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x002E3672 File Offset: 0x002E1A72
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060059FB RID: 23035 RVA: 0x002E3698 File Offset: 0x002E1A98
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x002E36B4 File Offset: 0x002E1AB4
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x002E36D0 File Offset: 0x002E1AD0
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

		// Token: 0x060059FE RID: 23038 RVA: 0x002E3740 File Offset: 0x002E1B40
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}
	}
}

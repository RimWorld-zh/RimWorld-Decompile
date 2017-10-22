namespace Verse
{
	public struct CellLine
	{
		private float zIntercept;

		private float slope;

		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num = (float)((a.x != b.x) ? ((float)(b.z - a.z) / (float)(b.x - a.x)) : 100000000.0);
			float num2 = (float)a.z - (float)a.x * num;
			return new CellLine(num2, num);
		}

		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}
	}
}

using System;

namespace RimWorld.Planet
{
	// Token: 0x020005B7 RID: 1463
	public struct TriangleIndices
	{
		// Token: 0x06001C20 RID: 7200 RVA: 0x000F2447 File Offset: 0x000F0847
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000F2460 File Offset: 0x000F0860
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000F2544 File Offset: 0x000F0944
		public int GetNextOrderedVertex(int root)
		{
			int result;
			if (this.v1 == root)
			{
				result = this.v2;
			}
			else if (this.v2 == root)
			{
				result = this.v3;
			}
			else
			{
				result = this.v1;
			}
			return result;
		}

		// Token: 0x040010DC RID: 4316
		public int v1;

		// Token: 0x040010DD RID: 4317
		public int v2;

		// Token: 0x040010DE RID: 4318
		public int v3;
	}
}

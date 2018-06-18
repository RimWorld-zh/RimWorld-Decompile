using System;

namespace RimWorld.Planet
{
	// Token: 0x020005BB RID: 1467
	public struct TriangleIndices
	{
		// Token: 0x06001C29 RID: 7209 RVA: 0x000F23F3 File Offset: 0x000F07F3
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000F240C File Offset: 0x000F080C
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000F24F0 File Offset: 0x000F08F0
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

		// Token: 0x040010DF RID: 4319
		public int v1;

		// Token: 0x040010E0 RID: 4320
		public int v2;

		// Token: 0x040010E1 RID: 4321
		public int v3;
	}
}

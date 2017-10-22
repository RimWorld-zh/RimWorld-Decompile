namespace RimWorld.Planet
{
	public struct TriangleIndices
	{
		public int v1;

		public int v2;

		public int v3;

		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			if (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3))
			{
				goto IL_00cc;
			}
			if (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3))
			{
				goto IL_00cc;
			}
			int result = (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3)) ? 1 : 0;
			goto IL_00cd;
			IL_00cd:
			return (byte)result != 0;
			IL_00cc:
			result = 1;
			goto IL_00cd;
		}

		public int GetNextOrderedVertex(int root)
		{
			return (this.v1 != root) ? ((this.v2 != root) ? this.v1 : this.v3) : this.v2;
		}
	}
}

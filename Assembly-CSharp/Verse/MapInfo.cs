using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C57 RID: 3159
	public sealed class MapInfo : IExposable
	{
		// Token: 0x04002F7C RID: 12156
		private IntVec3 sizeInt;

		// Token: 0x04002F7D RID: 12157
		public MapParent parent;

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x0024C394 File Offset: 0x0024A794
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x0024C3B4 File Offset: 0x0024A7B4
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x0024C3F8 File Offset: 0x0024A7F8
		// (set) Token: 0x06004588 RID: 17800 RVA: 0x0024C413 File Offset: 0x0024A813
		public IntVec3 Size
		{
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x0024C420 File Offset: 0x0024A820
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}
	}
}

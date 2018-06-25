using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C58 RID: 3160
	public sealed class MapInfo : IExposable
	{
		// Token: 0x04002F83 RID: 12163
		private IntVec3 sizeInt;

		// Token: 0x04002F84 RID: 12164
		public MapParent parent;

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x0024C674 File Offset: 0x0024AA74
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x0024C694 File Offset: 0x0024AA94
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x0024C6D8 File Offset: 0x0024AAD8
		// (set) Token: 0x06004588 RID: 17800 RVA: 0x0024C6F3 File Offset: 0x0024AAF3
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

		// Token: 0x06004589 RID: 17801 RVA: 0x0024C700 File Offset: 0x0024AB00
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}
	}
}

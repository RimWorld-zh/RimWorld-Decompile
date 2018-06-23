using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C55 RID: 3157
	public sealed class MapInfo : IExposable
	{
		// Token: 0x04002F7C RID: 12156
		private IntVec3 sizeInt;

		// Token: 0x04002F7D RID: 12157
		public MapParent parent;

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x0024C2B8 File Offset: 0x0024A6B8
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x0024C2D8 File Offset: 0x0024A6D8
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x0024C31C File Offset: 0x0024A71C
		// (set) Token: 0x06004585 RID: 17797 RVA: 0x0024C337 File Offset: 0x0024A737
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

		// Token: 0x06004586 RID: 17798 RVA: 0x0024C344 File Offset: 0x0024A744
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}
	}
}

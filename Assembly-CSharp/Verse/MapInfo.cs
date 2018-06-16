using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C59 RID: 3161
	public sealed class MapInfo : IExposable
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x0600457B RID: 17787 RVA: 0x0024AF10 File Offset: 0x00249310
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x0024AF30 File Offset: 0x00249330
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x0600457D RID: 17789 RVA: 0x0024AF74 File Offset: 0x00249374
		// (set) Token: 0x0600457E RID: 17790 RVA: 0x0024AF8F File Offset: 0x0024938F
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

		// Token: 0x0600457F RID: 17791 RVA: 0x0024AF9C File Offset: 0x0024939C
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04002F74 RID: 12148
		private IntVec3 sizeInt;

		// Token: 0x04002F75 RID: 12149
		public MapParent parent;
	}
}

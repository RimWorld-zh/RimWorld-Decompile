using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C58 RID: 3160
	public sealed class MapInfo : IExposable
	{
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004579 RID: 17785 RVA: 0x0024AEE8 File Offset: 0x002492E8
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x0024AF08 File Offset: 0x00249308
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x0600457B RID: 17787 RVA: 0x0024AF4C File Offset: 0x0024934C
		// (set) Token: 0x0600457C RID: 17788 RVA: 0x0024AF67 File Offset: 0x00249367
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

		// Token: 0x0600457D RID: 17789 RVA: 0x0024AF74 File Offset: 0x00249374
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04002F72 RID: 12146
		private IntVec3 sizeInt;

		// Token: 0x04002F73 RID: 12147
		public MapParent parent;
	}
}

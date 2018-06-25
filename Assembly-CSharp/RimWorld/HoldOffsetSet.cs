using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026D RID: 621
	public class HoldOffsetSet
	{
		// Token: 0x040004FE RID: 1278
		public HoldOffset northDefault = null;

		// Token: 0x040004FF RID: 1279
		public HoldOffset east = null;

		// Token: 0x04000500 RID: 1280
		public HoldOffset south = null;

		// Token: 0x04000501 RID: 1281
		public HoldOffset west = null;

		// Token: 0x06000AAB RID: 2731 RVA: 0x000606F0 File Offset: 0x0005EAF0
		public HoldOffset Pick(Rot4 rotation)
		{
			HoldOffset result;
			if (rotation == Rot4.North)
			{
				result = this.northDefault;
			}
			else if (rotation == Rot4.East)
			{
				result = this.east;
			}
			else if (rotation == Rot4.South)
			{
				result = this.south;
			}
			else if (rotation == Rot4.West)
			{
				result = this.west;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026D RID: 621
	public class HoldOffsetSet
	{
		// Token: 0x040004FC RID: 1276
		public HoldOffset northDefault = null;

		// Token: 0x040004FD RID: 1277
		public HoldOffset east = null;

		// Token: 0x040004FE RID: 1278
		public HoldOffset south = null;

		// Token: 0x040004FF RID: 1279
		public HoldOffset west = null;

		// Token: 0x06000AAC RID: 2732 RVA: 0x000606F4 File Offset: 0x0005EAF4
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

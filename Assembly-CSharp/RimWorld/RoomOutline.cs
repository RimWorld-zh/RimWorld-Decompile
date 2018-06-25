using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003E7 RID: 999
	public class RoomOutline
	{
		// Token: 0x04000A5C RID: 2652
		public CellRect rect;

		// Token: 0x06001117 RID: 4375 RVA: 0x00092653 File Offset: 0x00090A53
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00092664 File Offset: 0x00090A64
		public int CellsCountIgnoringWalls
		{
			get
			{
				int result;
				if (this.rect.Width <= 2 || this.rect.Height <= 2)
				{
					result = 0;
				}
				else
				{
					result = (this.rect.Width - 2) * (this.rect.Height - 2);
				}
				return result;
			}
		}
	}
}

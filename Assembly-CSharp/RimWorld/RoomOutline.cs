using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003E5 RID: 997
	public class RoomOutline
	{
		// Token: 0x04000A59 RID: 2649
		public CellRect rect;

		// Token: 0x06001114 RID: 4372 RVA: 0x000924F3 File Offset: 0x000908F3
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00092504 File Offset: 0x00090904
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

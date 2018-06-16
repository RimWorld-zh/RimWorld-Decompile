using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003E5 RID: 997
	public class RoomOutline
	{
		// Token: 0x06001114 RID: 4372 RVA: 0x00092307 File Offset: 0x00090707
		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00092318 File Offset: 0x00090718
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

		// Token: 0x04000A57 RID: 2647
		public CellRect rect;
	}
}

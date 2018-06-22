using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009F9 RID: 2553
	public static class LordUtility
	{
		// Token: 0x06003956 RID: 14678 RVA: 0x001E6FB4 File Offset: 0x001E53B4
		public static Lord GetLord(this Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(p);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}
	}
}

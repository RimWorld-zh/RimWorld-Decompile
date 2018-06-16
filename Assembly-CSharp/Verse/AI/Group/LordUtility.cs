using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FD RID: 2557
	public static class LordUtility
	{
		// Token: 0x0600395A RID: 14682 RVA: 0x001E6CA0 File Offset: 0x001E50A0
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

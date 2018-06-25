using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FC RID: 2556
	public static class LordUtility
	{
		// Token: 0x0600395B RID: 14683 RVA: 0x001E740C File Offset: 0x001E580C
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

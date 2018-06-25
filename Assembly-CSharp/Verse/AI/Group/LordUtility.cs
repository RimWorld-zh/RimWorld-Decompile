using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020009FB RID: 2555
	public static class LordUtility
	{
		// Token: 0x0600395A RID: 14682 RVA: 0x001E70E0 File Offset: 0x001E54E0
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

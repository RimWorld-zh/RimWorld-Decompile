using System.Collections.Generic;

namespace Verse.AI.Group
{
	public static class LordUtility
	{
		public static Lord GetLord(this Pawn p)
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			Lord result;
			while (true)
			{
				if (num < maps.Count)
				{
					Lord lord = maps[num].lordManager.LordOf(p);
					if (lord != null)
					{
						result = lord;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}

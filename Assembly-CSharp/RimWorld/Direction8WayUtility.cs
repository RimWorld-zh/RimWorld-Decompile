using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000976 RID: 2422
	public static class Direction8WayUtility
	{
		// Token: 0x06003681 RID: 13953 RVA: 0x001D0DDC File Offset: 0x001CF1DC
		public static string LabelShort(this Direction8Way dir)
		{
			string result;
			switch (dir)
			{
			case Direction8Way.North:
				result = "Direction8Way_North_Short".Translate();
				break;
			case Direction8Way.NorthEast:
				result = "Direction8Way_NorthEast_Short".Translate();
				break;
			case Direction8Way.East:
				result = "Direction8Way_East_Short".Translate();
				break;
			case Direction8Way.SouthEast:
				result = "Direction8Way_SouthEast_Short".Translate();
				break;
			case Direction8Way.South:
				result = "Direction8Way_South_Short".Translate();
				break;
			case Direction8Way.SouthWest:
				result = "Direction8Way_SouthWest_Short".Translate();
				break;
			case Direction8Way.West:
				result = "Direction8Way_West_Short".Translate();
				break;
			case Direction8Way.NorthWest:
				result = "Direction8Way_NorthWest_Short".Translate();
				break;
			default:
				result = "Unknown Direction8Way";
				break;
			}
			return result;
		}
	}
}

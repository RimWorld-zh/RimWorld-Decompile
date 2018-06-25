using System;
using Verse;

namespace RimWorld
{
	public class HoldOffsetSet
	{
		public HoldOffset northDefault = null;

		public HoldOffset east = null;

		public HoldOffset south = null;

		public HoldOffset west = null;

		public HoldOffsetSet()
		{
		}

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

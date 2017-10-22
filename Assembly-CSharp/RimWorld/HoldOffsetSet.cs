using Verse;

namespace RimWorld
{
	public class HoldOffsetSet
	{
		public HoldOffset northDefault = null;

		public HoldOffset east = null;

		public HoldOffset south = null;

		public HoldOffset west = null;

		public HoldOffset Pick(Rot4 rotation)
		{
			return (!(rotation == Rot4.North)) ? ((!(rotation == Rot4.East)) ? ((!(rotation == Rot4.South)) ? ((!(rotation == Rot4.West)) ? null : this.west) : this.south) : this.east) : this.northDefault;
		}
	}
}

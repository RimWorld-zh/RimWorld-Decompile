namespace Verse.AI
{
	public static class PawnLocalAwareness
	{
		private const float SightRadius = 30f;

		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			if (!p.RaceProps.ToolUser && p.Faction == null)
			{
				if ((float)(p.Position - t.Position).LengthHorizontalSquared > 900.0)
				{
					return false;
				}
				if (p.GetRoom(RegionType.Set_Passable) != t.GetRoom(RegionType.Set_Passable))
				{
					return false;
				}
				if (!GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0))
				{
					return false;
				}
				return true;
			}
			return true;
		}
	}
}

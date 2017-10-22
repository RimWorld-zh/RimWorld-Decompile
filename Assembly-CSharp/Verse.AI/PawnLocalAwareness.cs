namespace Verse.AI
{
	public static class PawnLocalAwareness
	{
		private const float SightRadius = 30f;

		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return (byte)((p.RaceProps.ToolUser || p.Faction != null) ? 1 : ((!((float)(p.Position - t.Position).LengthHorizontalSquared > 900.0)) ? ((p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable)) ? (GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0) ? 1 : 0) : 0) : 0)) != 0;
		}
	}
}

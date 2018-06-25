using System;

namespace Verse.AI
{
	// Token: 0x02000AA0 RID: 2720
	public static class PawnLocalAwareness
	{
		// Token: 0x04002653 RID: 9811
		private const float SightRadius = 30f;

		// Token: 0x06003C93 RID: 15507 RVA: 0x0020082C File Offset: 0x001FEC2C
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}
	}
}

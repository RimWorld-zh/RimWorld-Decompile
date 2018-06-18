using System;

namespace Verse.AI
{
	// Token: 0x02000AA2 RID: 2722
	public static class PawnLocalAwareness
	{
		// Token: 0x06003C94 RID: 15508 RVA: 0x002003DC File Offset: 0x001FE7DC
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}

		// Token: 0x04002657 RID: 9815
		private const float SightRadius = 30f;
	}
}

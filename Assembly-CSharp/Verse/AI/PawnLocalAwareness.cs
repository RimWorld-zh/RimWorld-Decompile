using System;

namespace Verse.AI
{
	// Token: 0x02000AA1 RID: 2721
	public static class PawnLocalAwareness
	{
		// Token: 0x04002663 RID: 9827
		private const float SightRadius = 30f;

		// Token: 0x06003C94 RID: 15508 RVA: 0x00200B58 File Offset: 0x001FEF58
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}
	}
}

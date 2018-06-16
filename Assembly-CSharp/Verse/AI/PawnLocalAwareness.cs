using System;

namespace Verse.AI
{
	// Token: 0x02000AA2 RID: 2722
	public static class PawnLocalAwareness
	{
		// Token: 0x06003C92 RID: 15506 RVA: 0x00200308 File Offset: 0x001FE708
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}

		// Token: 0x04002657 RID: 9815
		private const float SightRadius = 30f;
	}
}

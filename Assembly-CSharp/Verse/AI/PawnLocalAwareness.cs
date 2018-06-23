using System;

namespace Verse.AI
{
	// Token: 0x02000A9E RID: 2718
	public static class PawnLocalAwareness
	{
		// Token: 0x04002652 RID: 9810
		private const float SightRadius = 30f;

		// Token: 0x06003C8F RID: 15503 RVA: 0x00200700 File Offset: 0x001FEB00
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}
	}
}

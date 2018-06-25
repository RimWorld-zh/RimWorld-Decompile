using System;

namespace RimWorld
{
	// Token: 0x0200094C RID: 2380
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x0400226B RID: 8811
		public static WorldObjectDef Caravan;

		// Token: 0x0400226C RID: 8812
		public static WorldObjectDef FactionBase;

		// Token: 0x0400226D RID: 8813
		public static WorldObjectDef AbandonedBase;

		// Token: 0x0400226E RID: 8814
		public static WorldObjectDef EscapeShip;

		// Token: 0x0400226F RID: 8815
		public static WorldObjectDef Ambush;

		// Token: 0x04002270 RID: 8816
		public static WorldObjectDef DestroyedFactionBase;

		// Token: 0x04002271 RID: 8817
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x04002272 RID: 8818
		public static WorldObjectDef TravelingTransportPods;

		// Token: 0x04002273 RID: 8819
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x04002274 RID: 8820
		public static WorldObjectDef Site;

		// Token: 0x04002275 RID: 8821
		public static WorldObjectDef PeaceTalks;

		// Token: 0x04002276 RID: 8822
		public static WorldObjectDef Debug_Arena;

		// Token: 0x06003656 RID: 13910 RVA: 0x001D1095 File Offset: 0x001CF495
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}
	}
}

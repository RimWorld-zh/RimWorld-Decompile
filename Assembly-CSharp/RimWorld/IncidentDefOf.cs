using System;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	[DefOf]
	public static class IncidentDefOf
	{
		// Token: 0x040021C4 RID: 8644
		public static IncidentDef RaidEnemy;

		// Token: 0x040021C5 RID: 8645
		public static IncidentDef RaidFriendly;

		// Token: 0x040021C6 RID: 8646
		public static IncidentDef VisitorGroup;

		// Token: 0x040021C7 RID: 8647
		public static IncidentDef TravelerGroup;

		// Token: 0x040021C8 RID: 8648
		public static IncidentDef TraderCaravanArrival;

		// Token: 0x040021C9 RID: 8649
		public static IncidentDef Eclipse;

		// Token: 0x040021CA RID: 8650
		public static IncidentDef ToxicFallout;

		// Token: 0x040021CB RID: 8651
		public static IncidentDef SolarFlare;

		// Token: 0x040021CC RID: 8652
		public static IncidentDef ManhunterPack;

		// Token: 0x040021CD RID: 8653
		public static IncidentDef ShipChunkDrop;

		// Token: 0x040021CE RID: 8654
		public static IncidentDef OrbitalTraderArrival;

		// Token: 0x040021CF RID: 8655
		public static IncidentDef WandererJoin;

		// Token: 0x040021D0 RID: 8656
		public static IncidentDef Quest_TradeRequest;

		// Token: 0x040021D1 RID: 8657
		public static IncidentDef Quest_ItemStashAICore;

		// Token: 0x040021D2 RID: 8658
		public static IncidentDef Quest_JourneyOffer;

		// Token: 0x0600364F RID: 13903 RVA: 0x001D1017 File Offset: 0x001CF417
		static IncidentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentDefOf));
		}
	}
}

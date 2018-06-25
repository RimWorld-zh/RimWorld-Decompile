using System;

namespace RimWorld
{
	// Token: 0x02000963 RID: 2403
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x040022CD RID: 8909
		public static StoryEventDef DamageTaken;

		// Token: 0x040022CE RID: 8910
		public static StoryEventDef DamageDealt;

		// Token: 0x040022CF RID: 8911
		public static StoryEventDef AttackedPlayer;

		// Token: 0x040022D0 RID: 8912
		public static StoryEventDef KilledPlayer;

		// Token: 0x040022D1 RID: 8913
		public static StoryEventDef TendedByPlayer;

		// Token: 0x040022D2 RID: 8914
		public static StoryEventDef Seen;

		// Token: 0x040022D3 RID: 8915
		public static StoryEventDef TaleCreated;

		// Token: 0x0600366D RID: 13933 RVA: 0x001D0F5F File Offset: 0x001CF35F
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}
	}
}

using System;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x001D0E1F File Offset: 0x001CF21F
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}

		// Token: 0x040022CC RID: 8908
		public static StoryEventDef DamageTaken;

		// Token: 0x040022CD RID: 8909
		public static StoryEventDef DamageDealt;

		// Token: 0x040022CE RID: 8910
		public static StoryEventDef AttackedPlayer;

		// Token: 0x040022CF RID: 8911
		public static StoryEventDef KilledPlayer;

		// Token: 0x040022D0 RID: 8912
		public static StoryEventDef TendedByPlayer;

		// Token: 0x040022D1 RID: 8913
		public static StoryEventDef Seen;

		// Token: 0x040022D2 RID: 8914
		public static StoryEventDef TaleCreated;
	}
}

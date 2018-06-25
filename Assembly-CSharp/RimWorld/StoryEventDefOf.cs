using System;

namespace RimWorld
{
	// Token: 0x02000963 RID: 2403
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x040022D4 RID: 8916
		public static StoryEventDef DamageTaken;

		// Token: 0x040022D5 RID: 8917
		public static StoryEventDef DamageDealt;

		// Token: 0x040022D6 RID: 8918
		public static StoryEventDef AttackedPlayer;

		// Token: 0x040022D7 RID: 8919
		public static StoryEventDef KilledPlayer;

		// Token: 0x040022D8 RID: 8920
		public static StoryEventDef TendedByPlayer;

		// Token: 0x040022D9 RID: 8921
		public static StoryEventDef Seen;

		// Token: 0x040022DA RID: 8922
		public static StoryEventDef TaleCreated;

		// Token: 0x0600366D RID: 13933 RVA: 0x001D1233 File Offset: 0x001CF633
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}
	}
}

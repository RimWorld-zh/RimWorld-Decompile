using System;

namespace RimWorld
{
	// Token: 0x02000965 RID: 2405
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x06003670 RID: 13936 RVA: 0x001D0C37 File Offset: 0x001CF037
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}

		// Token: 0x040022CE RID: 8910
		public static StoryEventDef DamageTaken;

		// Token: 0x040022CF RID: 8911
		public static StoryEventDef DamageDealt;

		// Token: 0x040022D0 RID: 8912
		public static StoryEventDef AttackedPlayer;

		// Token: 0x040022D1 RID: 8913
		public static StoryEventDef KilledPlayer;

		// Token: 0x040022D2 RID: 8914
		public static StoryEventDef TendedByPlayer;

		// Token: 0x040022D3 RID: 8915
		public static StoryEventDef Seen;

		// Token: 0x040022D4 RID: 8916
		public static StoryEventDef TaleCreated;
	}
}

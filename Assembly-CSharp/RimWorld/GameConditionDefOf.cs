using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x04001EB1 RID: 7857
		public static GameConditionDef SolarFlare;

		// Token: 0x04001EB2 RID: 7858
		public static GameConditionDef Eclipse;

		// Token: 0x04001EB3 RID: 7859
		public static GameConditionDef PsychicDrone;

		// Token: 0x04001EB4 RID: 7860
		public static GameConditionDef PsychicSoothe;

		// Token: 0x04001EB5 RID: 7861
		public static GameConditionDef HeatWave;

		// Token: 0x04001EB6 RID: 7862
		public static GameConditionDef ColdSnap;

		// Token: 0x04001EB7 RID: 7863
		public static GameConditionDef Flashstorm;

		// Token: 0x04001EB8 RID: 7864
		public static GameConditionDef VolcanicWinter;

		// Token: 0x04001EB9 RID: 7865
		public static GameConditionDef ToxicFallout;

		// Token: 0x04001EBA RID: 7866
		public static GameConditionDef Aurora;

		// Token: 0x06003624 RID: 13860 RVA: 0x001D0D11 File Offset: 0x001CF111
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}
	}
}

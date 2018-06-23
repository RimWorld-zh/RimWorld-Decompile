using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x04001EAA RID: 7850
		public static GameConditionDef SolarFlare;

		// Token: 0x04001EAB RID: 7851
		public static GameConditionDef Eclipse;

		// Token: 0x04001EAC RID: 7852
		public static GameConditionDef PsychicDrone;

		// Token: 0x04001EAD RID: 7853
		public static GameConditionDef PsychicSoothe;

		// Token: 0x04001EAE RID: 7854
		public static GameConditionDef HeatWave;

		// Token: 0x04001EAF RID: 7855
		public static GameConditionDef ColdSnap;

		// Token: 0x04001EB0 RID: 7856
		public static GameConditionDef Flashstorm;

		// Token: 0x04001EB1 RID: 7857
		public static GameConditionDef VolcanicWinter;

		// Token: 0x04001EB2 RID: 7858
		public static GameConditionDef ToxicFallout;

		// Token: 0x04001EB3 RID: 7859
		public static GameConditionDef Aurora;

		// Token: 0x06003620 RID: 13856 RVA: 0x001D08FD File Offset: 0x001CECFD
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}
	}
}

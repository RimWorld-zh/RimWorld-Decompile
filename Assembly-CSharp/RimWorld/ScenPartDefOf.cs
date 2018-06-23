using System;

namespace RimWorld
{
	// Token: 0x0200091C RID: 2332
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x04001ED0 RID: 7888
		public static ScenPartDef PlayerFaction;

		// Token: 0x04001ED1 RID: 7889
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x04001ED2 RID: 7890
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x04001ED3 RID: 7891
		public static ScenPartDef ForcedTrait;

		// Token: 0x04001ED4 RID: 7892
		public static ScenPartDef ForcedHediff;

		// Token: 0x04001ED5 RID: 7893
		public static ScenPartDef StartingAnimal;

		// Token: 0x04001ED6 RID: 7894
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x04001ED7 RID: 7895
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x04001ED8 RID: 7896
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x04001ED9 RID: 7897
		public static ScenPartDef GameStartDialog;

		// Token: 0x06003624 RID: 13860 RVA: 0x001D0945 File Offset: 0x001CED45
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}
	}
}

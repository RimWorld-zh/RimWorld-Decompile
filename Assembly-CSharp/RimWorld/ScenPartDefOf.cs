using System;

namespace RimWorld
{
	// Token: 0x02000920 RID: 2336
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x06003629 RID: 13865 RVA: 0x001D0695 File Offset: 0x001CEA95
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x04001ED2 RID: 7890
		public static ScenPartDef PlayerFaction;

		// Token: 0x04001ED3 RID: 7891
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x04001ED4 RID: 7892
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x04001ED5 RID: 7893
		public static ScenPartDef ForcedTrait;

		// Token: 0x04001ED6 RID: 7894
		public static ScenPartDef ForcedHediff;

		// Token: 0x04001ED7 RID: 7895
		public static ScenPartDef StartingAnimal;

		// Token: 0x04001ED8 RID: 7896
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x04001ED9 RID: 7897
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x04001EDA RID: 7898
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x04001EDB RID: 7899
		public static ScenPartDef GameStartDialog;
	}
}

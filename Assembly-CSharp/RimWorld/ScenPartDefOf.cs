using System;

namespace RimWorld
{
	// Token: 0x0200091E RID: 2334
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x04001ED7 RID: 7895
		public static ScenPartDef PlayerFaction;

		// Token: 0x04001ED8 RID: 7896
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x04001ED9 RID: 7897
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x04001EDA RID: 7898
		public static ScenPartDef ForcedTrait;

		// Token: 0x04001EDB RID: 7899
		public static ScenPartDef ForcedHediff;

		// Token: 0x04001EDC RID: 7900
		public static ScenPartDef StartingAnimal;

		// Token: 0x04001EDD RID: 7901
		public static ScenPartDef ScatterThingsNearPlayerStart;

		// Token: 0x04001EDE RID: 7902
		public static ScenPartDef StartingThing_Defined;

		// Token: 0x04001EDF RID: 7903
		public static ScenPartDef ScatterThingsAnywhere;

		// Token: 0x04001EE0 RID: 7904
		public static ScenPartDef GameStartDialog;

		// Token: 0x06003628 RID: 13864 RVA: 0x001D0D59 File Offset: 0x001CF159
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}
	}
}

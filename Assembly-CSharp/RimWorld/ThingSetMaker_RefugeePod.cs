using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FC RID: 1788
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x060026ED RID: 9965 RVA: 0x0014E0D8 File Offset: 0x0014C4D8
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x0014E150 File Offset: 0x0014C550
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x040015A7 RID: 5543
		private const float RelationWithColonistWeight = 20f;
	}
}

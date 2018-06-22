using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F8 RID: 1784
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x060026E7 RID: 9959 RVA: 0x0014E2F4 File Offset: 0x0014C6F4
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn);
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0014E36C File Offset: 0x0014C76C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x040015A5 RID: 5541
		private const float RelationWithColonistWeight = 20f;
	}
}

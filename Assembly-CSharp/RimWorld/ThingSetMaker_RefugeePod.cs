using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FA RID: 1786
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x040015A9 RID: 5545
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x060026EA RID: 9962 RVA: 0x0014E6A4 File Offset: 0x0014CAA4
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn);
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x0014E71C File Offset: 0x0014CB1C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}
	}
}

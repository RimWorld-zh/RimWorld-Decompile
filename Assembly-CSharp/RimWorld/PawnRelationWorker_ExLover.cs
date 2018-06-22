using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C2 RID: 1218
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060015CF RID: 5583 RVA: 0x000C25C4 File Offset: 0x000C09C4
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x000C25EC File Offset: 0x000C09EC
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x000C261C File Offset: 0x000C0A1C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

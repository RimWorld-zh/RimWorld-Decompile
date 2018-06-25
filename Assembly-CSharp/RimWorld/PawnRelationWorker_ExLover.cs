using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C4 RID: 1220
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060015D2 RID: 5586 RVA: 0x000C2914 File Offset: 0x000C0D14
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x000C293C File Offset: 0x000C0D3C
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x000C296C File Offset: 0x000C0D6C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

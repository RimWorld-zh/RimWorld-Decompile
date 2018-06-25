using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C4 RID: 1220
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x000C2714 File Offset: 0x000C0B14
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x000C273C File Offset: 0x000C0B3C
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000C276C File Offset: 0x000C0B6C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C6 RID: 1222
	public class PawnRelationWorker_ExLover : PawnRelationWorker
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x000C25D4 File Offset: 0x000C09D4
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000C25FC File Offset: 0x000C09FC
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_ExLover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000C262C File Offset: 0x000C0A2C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

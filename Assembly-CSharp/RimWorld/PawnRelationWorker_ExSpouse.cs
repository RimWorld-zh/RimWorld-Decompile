using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C3 RID: 1219
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x000C266C File Offset: 0x000C0A6C
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x000C2694 File Offset: 0x000C0A94
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000C26C4 File Offset: 0x000C0AC4
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

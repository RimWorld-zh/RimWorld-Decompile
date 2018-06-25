using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CF RID: 1231
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x060015EF RID: 5615 RVA: 0x000C2EC0 File Offset: 0x000C12C0
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x000C2EE8 File Offset: 0x000C12E8
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_Lover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000C2F18 File Offset: 0x000C1318
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

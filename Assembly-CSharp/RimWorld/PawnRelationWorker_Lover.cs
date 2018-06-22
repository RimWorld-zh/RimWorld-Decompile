using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CD RID: 1229
	public class PawnRelationWorker_Lover : PawnRelationWorker
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x000C2B70 File Offset: 0x000C0F70
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000C2B98 File Offset: 0x000C0F98
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Lover, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.35f);
			PawnRelationWorker_Lover.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000C2BC8 File Offset: 0x000C0FC8
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

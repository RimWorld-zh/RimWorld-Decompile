using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C7 RID: 1223
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x000C267C File Offset: 0x000C0A7C
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x000C26A4 File Offset: 0x000C0AA4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x000C26D4 File Offset: 0x000C0AD4
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

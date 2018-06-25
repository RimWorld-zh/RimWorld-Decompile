using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C5 RID: 1221
	public class PawnRelationWorker_ExSpouse : PawnRelationWorker
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x000C29BC File Offset: 0x000C0DBC
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, true) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x000C29E4 File Offset: 0x000C0DE4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_ExSpouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000C2A14 File Offset: 0x000C0E14
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

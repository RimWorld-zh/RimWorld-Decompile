using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C4 RID: 1220
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x060015D7 RID: 5591 RVA: 0x000C2714 File Offset: 0x000C0B14
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000C2758 File Offset: 0x000C0B58
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
			PawnRelationWorker_Fiance.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000C2788 File Offset: 0x000C0B88
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000C27D4 File Offset: 0x000C0BD4
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

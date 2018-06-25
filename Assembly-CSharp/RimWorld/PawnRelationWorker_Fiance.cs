using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C6 RID: 1222
	public class PawnRelationWorker_Fiance : PawnRelationWorker
	{
		// Token: 0x060015DA RID: 5594 RVA: 0x000C2A64 File Offset: 0x000C0E64
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			num *= this.GetOldAgeFactor(generated);
			num *= this.GetOldAgeFactor(other);
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request) * num;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000C2AA8 File Offset: 0x000C0EA8
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Fiance, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 0.7f);
			PawnRelationWorker_Fiance.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x000C2AD8 File Offset: 0x000C0ED8
		private float GetOldAgeFactor(Pawn pawn)
		{
			return Mathf.Clamp(GenMath.LerpDouble(50f, 80f, 1f, 0.01f, (float)pawn.ageTracker.AgeBiologicalYears), 0.01f, 1f);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x000C2B24 File Offset: 0x000C0F24
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

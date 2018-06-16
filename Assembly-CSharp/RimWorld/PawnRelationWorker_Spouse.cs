using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D7 RID: 1239
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x000C3920 File Offset: 0x000C1D20
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x000C3948 File Offset: 0x000C1D48
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_Spouse.ResolveMyName(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x000C397D File Offset: 0x000C1D7D
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName == null)
			{
				if (Rand.Value < 0.8f)
				{
					request.SetFixedLastName(((NameTriple)generated.GetSpouse().Name).Last);
				}
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000C39BC File Offset: 0x000C1DBC
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

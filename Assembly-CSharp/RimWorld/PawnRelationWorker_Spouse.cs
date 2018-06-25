using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D5 RID: 1237
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x06001607 RID: 5639 RVA: 0x000C3A7C File Offset: 0x000C1E7C
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000C3AA4 File Offset: 0x000C1EA4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_Spouse.ResolveMyName(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x000C3AD9 File Offset: 0x000C1ED9
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

		// Token: 0x0600160A RID: 5642 RVA: 0x000C3B18 File Offset: 0x000C1F18
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

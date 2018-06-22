using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D3 RID: 1235
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x06001603 RID: 5635 RVA: 0x000C392C File Offset: 0x000C1D2C
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000C3954 File Offset: 0x000C1D54
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_Spouse.ResolveMyName(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000C3989 File Offset: 0x000C1D89
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

		// Token: 0x06001606 RID: 5638 RVA: 0x000C39C8 File Offset: 0x000C1DC8
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

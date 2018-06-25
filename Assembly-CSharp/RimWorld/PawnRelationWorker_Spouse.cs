using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D5 RID: 1237
	public class PawnRelationWorker_Spouse : PawnRelationWorker
	{
		// Token: 0x06001606 RID: 5638 RVA: 0x000C3C7C File Offset: 0x000C207C
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return LovePartnerRelationUtility.LovePartnerRelationGenerationChance(generated, other, request, false) * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x000C3CA4 File Offset: 0x000C20A4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other);
			LovePartnerRelationUtility.TryToShareChildrenForGeneratedLovePartner(generated, other, request, 1f);
			PawnRelationWorker_Spouse.ResolveMyName(ref request, generated);
			PawnRelationWorker_Spouse.ResolveMySkinColor(ref request, generated, other);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000C3CD9 File Offset: 0x000C20D9
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

		// Token: 0x06001609 RID: 5641 RVA: 0x000C3D18 File Offset: 0x000C2118
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generated, Pawn other)
		{
			if (request.FixedMelanin == null)
			{
				request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(other.story.melanin, 0f, 1f));
			}
		}
	}
}

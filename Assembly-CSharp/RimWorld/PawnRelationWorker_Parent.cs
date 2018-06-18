using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D3 RID: 1235
	public class PawnRelationWorker_Parent : PawnRelationWorker
	{
		// Token: 0x060015FB RID: 5627 RVA: 0x000C2CF0 File Offset: 0x000C10F0
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (other.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other, other.GetSpouseOppositeGender(), new PawnGenerationRequest?(request), null, null);
			}
			else if (other.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetSpouseOppositeGender(), other, new PawnGenerationRequest?(request), null, null);
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x000C2D80 File Offset: 0x000C1180
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (other.gender == Gender.Male)
			{
				generated.SetFather(other);
				Pawn spouseOppositeGender = other.GetSpouseOppositeGender();
				if (spouseOppositeGender != null)
				{
					generated.SetMother(spouseOppositeGender);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
			}
			else if (other.gender == Gender.Female)
			{
				generated.SetMother(other);
				Pawn spouseOppositeGender2 = other.GetSpouseOppositeGender();
				if (spouseOppositeGender2 != null)
				{
					generated.SetFather(spouseOppositeGender2);
				}
				PawnRelationWorker_Parent.ResolveMyName(ref request, generated);
				PawnRelationWorker_Parent.ResolveMySkinColor(ref request, generated);
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x000C2E04 File Offset: 0x000C1204
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generatedChild)
		{
			if (request.FixedLastName == null)
			{
				if (ChildRelationUtility.ChildWantsNameOfAnyParent(generatedChild))
				{
					bool flag = Rand.Value < 0.5f || generatedChild.GetMother() == null;
					if (generatedChild.GetFather() == null)
					{
						flag = false;
					}
					if (flag)
					{
						request.SetFixedLastName(((NameTriple)generatedChild.GetFather().Name).Last);
					}
					else
					{
						request.SetFixedLastName(((NameTriple)generatedChild.GetMother().Name).Last);
					}
				}
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x000C2E9C File Offset: 0x000C129C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn generatedChild)
		{
			if (request.FixedMelanin == null)
			{
				if (generatedChild.GetFather() != null && generatedChild.GetMother() != null)
				{
					request.SetFixedMelanin(ChildRelationUtility.GetRandomChildSkinColor(generatedChild.GetFather().story.melanin, generatedChild.GetMother().story.melanin));
				}
				else if (generatedChild.GetFather() != null)
				{
					request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(generatedChild.GetFather().story.melanin, 0f, 1f));
				}
				else
				{
					request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(generatedChild.GetMother().story.melanin, 0f, 1f));
				}
			}
		}
	}
}

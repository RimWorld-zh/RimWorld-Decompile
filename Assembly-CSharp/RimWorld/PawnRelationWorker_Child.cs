using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BE RID: 1214
	public class PawnRelationWorker_Child : PawnRelationWorker
	{
		// Token: 0x060015C3 RID: 5571 RVA: 0x000C2018 File Offset: 0x000C0418
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && (other.GetMother() == me || other.GetFather() == me);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x000C2054 File Offset: 0x000C0454
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 0f;
			if (generated.gender == Gender.Male)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, generated, other.GetMother(), null, new PawnGenerationRequest?(request), null);
			}
			else if (generated.gender == Gender.Female)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(other, other.GetFather(), generated, null, null, new PawnGenerationRequest?(request));
			}
			return num * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x000C20E4 File Offset: 0x000C04E4
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (generated.gender == Gender.Male)
			{
				other.SetFather(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetMother());
				PawnRelationWorker_Child.ResolveMySkinColor(ref request, other, other.GetMother());
				if (other.GetMother() != null)
				{
					if (other.GetMother().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetMother());
					}
					else if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetMother()))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetMother());
						if (request.FixedLastName == null && Rand.Value < 0.8f)
						{
							request.SetFixedLastName(((NameTriple)other.GetMother().Name).Last);
						}
					}
					else
					{
						LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetMother());
					}
				}
			}
			else if (generated.gender == Gender.Female)
			{
				other.SetMother(generated);
				PawnRelationWorker_Child.ResolveMyName(ref request, other, other.GetFather());
				PawnRelationWorker_Child.ResolveMySkinColor(ref request, other, other.GetFather());
				if (other.GetFather() != null)
				{
					if (other.GetFather().story.traits.HasTrait(TraitDefOf.Gay))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetFather());
					}
					else if (Rand.Value < 0.85f && !LovePartnerRelationUtility.HasAnyLovePartner(other.GetFather()))
					{
						generated.relations.AddDirectRelation(PawnRelationDefOf.Spouse, other.GetFather());
						if (request.FixedLastName == null && Rand.Value < 0.8f)
						{
							request.SetFixedLastName(((NameTriple)other.GetFather().Name).Last);
						}
					}
					else
					{
						LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(generated, other.GetFather());
					}
				}
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000C22E4 File Offset: 0x000C06E4
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn child, Pawn otherParent)
		{
			if (request.FixedLastName == null)
			{
				if (!ChildRelationUtility.DefinitelyHasNotBirthName(child))
				{
					if (ChildRelationUtility.ChildWantsNameOfAnyParent(child))
					{
						if (otherParent == null)
						{
							float num = 0.9f;
							if (Rand.Value < num)
							{
								request.SetFixedLastName(((NameTriple)child.Name).Last);
							}
						}
						else
						{
							string last = ((NameTriple)child.Name).Last;
							string last2 = ((NameTriple)otherParent.Name).Last;
							if (last != last2)
							{
								request.SetFixedLastName(last);
							}
						}
					}
				}
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x000C238C File Offset: 0x000C078C
		private static void ResolveMySkinColor(ref PawnGenerationRequest request, Pawn child, Pawn otherParent)
		{
			if (request.FixedMelanin == null)
			{
				if (otherParent != null)
				{
					request.SetFixedMelanin(ParentRelationUtility.GetRandomSecondParentSkinColor(otherParent.story.melanin, child.story.melanin, null));
				}
				else
				{
					request.SetFixedMelanin(PawnSkinColors.GetRandomMelaninSimilarTo(child.story.melanin, 0f, 1f));
				}
			}
		}
	}
}

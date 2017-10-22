using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(base.pawn, false);
				return string.Format(base.CurStage.label, directPawnRelation.def.GetGenderSpecificLabel(directPawnRelation.otherPawn), directPawnRelation.otherPawn.LabelShort).CapitalizeFirst();
			}
		}

		protected override float BaseMoodOffset
		{
			get
			{
				float num = (float)(0.10000000149011612 * (float)base.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(base.pawn, false).otherPawn));
				if (num < 0.0)
				{
					return Mathf.Min(num, -1f);
				}
				return Mathf.Max(num, 1f);
			}
		}
	}
}

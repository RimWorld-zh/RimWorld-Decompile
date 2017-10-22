using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_WantToSleepWithSpouseOrLover : Thought_Situational
	{
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(base.pawn, false);
				return string.Format(base.CurStage.label, directPawnRelation.otherPawn.LabelShort).CapitalizeFirst();
			}
		}

		protected override float BaseMoodOffset
		{
			get
			{
				float a = (float)(-0.05000000074505806 * (float)base.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(base.pawn, false).otherPawn));
				return Mathf.Min(a, -1f);
			}
		}
	}
}

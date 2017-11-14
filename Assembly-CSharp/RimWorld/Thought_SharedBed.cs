using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_SharedBed : Thought_Situational
	{
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(base.pawn);
				if (mostDislikedNonPartnerBedOwner == null)
				{
					return 0f;
				}
				return Mathf.Min((float)(0.05000000074505806 * (float)base.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5.0), 0f);
			}
		}
	}
}

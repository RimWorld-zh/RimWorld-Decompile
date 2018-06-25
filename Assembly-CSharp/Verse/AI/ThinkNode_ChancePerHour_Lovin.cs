using System;
using RimWorld;

namespace Verse.AI
{
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		public ThinkNode_ChancePerHour_Lovin()
		{
		}

		protected override float MtbHours(Pawn pawn)
		{
			float result;
			if (pawn.CurrentBed() == null)
			{
				result = -1f;
			}
			else
			{
				Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
				if (partnerInMyBed == null)
				{
					result = -1f;
				}
				else
				{
					result = LovePartnerRelationUtility.GetLovinMtbHours(pawn, partnerInMyBed);
				}
			}
			return result;
		}
	}
}

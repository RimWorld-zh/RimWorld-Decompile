using RimWorld;

namespace Verse.AI
{
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
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
				result = (float)((partnerInMyBed != null) ? LovePartnerRelationUtility.GetLovinMtbHours(pawn, partnerInMyBed) : -1.0);
			}
			return result;
		}
	}
}

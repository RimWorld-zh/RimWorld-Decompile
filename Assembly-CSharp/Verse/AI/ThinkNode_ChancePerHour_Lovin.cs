using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AB1 RID: 2737
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D13 RID: 15635 RVA: 0x002047AC File Offset: 0x00202BAC
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

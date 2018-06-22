using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AAD RID: 2733
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D10 RID: 15632 RVA: 0x00204BA4 File Offset: 0x00202FA4
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

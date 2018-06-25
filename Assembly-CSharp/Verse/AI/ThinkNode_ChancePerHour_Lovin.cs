using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D14 RID: 15636 RVA: 0x00204CD0 File Offset: 0x002030D0
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

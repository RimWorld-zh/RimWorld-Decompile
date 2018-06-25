using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AB0 RID: 2736
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D14 RID: 15636 RVA: 0x00204FB0 File Offset: 0x002033B0
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

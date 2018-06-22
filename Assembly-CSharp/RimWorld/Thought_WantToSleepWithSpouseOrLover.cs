using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F9 RID: 505
	public class Thought_WantToSleepWithSpouseOrLover : Thought_Situational
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x00057C58 File Offset: 0x00056058
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				return string.Format(base.CurStage.label, directPawnRelation.otherPawn.LabelShort).CapitalizeFirst();
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x00057C9C File Offset: 0x0005609C
		protected override float BaseMoodOffset
		{
			get
			{
				float a = -0.05f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn);
				return Mathf.Min(a, -1f);
			}
		}
	}
}

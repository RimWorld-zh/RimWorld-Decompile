using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021F RID: 543
	public class Thought_SharedBed : Thought_Situational
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000593C4 File Offset: 0x000577C4
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(this.pawn);
				float result;
				if (mostDislikedNonPartnerBedOwner == null)
				{
					result = 0f;
				}
				else
				{
					result = Mathf.Min(0.05f * (float)this.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5f, 0f);
				}
				return result;
			}
		}
	}
}

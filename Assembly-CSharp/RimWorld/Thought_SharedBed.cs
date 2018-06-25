using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_SharedBed : Thought_Situational
	{
		public Thought_SharedBed()
		{
		}

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

using System;
using Verse;

namespace RimWorld
{
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		public TransferableComparer_HitPointsPercentage()
		{
		}

		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			float result;
			if (pawn != null)
			{
				result = pawn.health.summaryHealth.SummaryHealthPercent;
			}
			else if (!anyThing.def.useHitPoints)
			{
				result = 1f;
			}
			else
			{
				result = (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
			}
			return result;
		}
	}
}

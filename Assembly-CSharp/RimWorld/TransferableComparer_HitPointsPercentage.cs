using Verse;

namespace RimWorld
{
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			return (float)((pawn == null) ? (anyThing.def.useHitPoints ? ((float)anyThing.HitPoints / (float)anyThing.MaxHitPoints) : 1.0) : pawn.health.summaryHealth.SummaryHealthPercent);
		}
	}
}

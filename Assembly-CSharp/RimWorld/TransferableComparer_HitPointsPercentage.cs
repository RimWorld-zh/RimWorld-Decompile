using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AF RID: 2223
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x001B6454 File Offset: 0x001B4854
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x001B6480 File Offset: 0x001B4880
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

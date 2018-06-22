using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AB RID: 2219
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x060032D4 RID: 13012 RVA: 0x001B663C File Offset: 0x001B4A3C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x001B6668 File Offset: 0x001B4A68
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

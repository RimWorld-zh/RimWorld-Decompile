using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AD RID: 2221
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x060032D8 RID: 13016 RVA: 0x001B6A50 File Offset: 0x001B4E50
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x001B6A7C File Offset: 0x001B4E7C
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

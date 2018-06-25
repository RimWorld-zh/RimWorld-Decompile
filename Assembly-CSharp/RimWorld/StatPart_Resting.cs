using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B6 RID: 2486
	public class StatPart_Resting : StatPart
	{
		// Token: 0x040023B9 RID: 9145
		public float factor = 1f;

		// Token: 0x060037AF RID: 14255 RVA: 0x001DA954 File Offset: 0x001D8D54
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.RestingMultiplier(pawn);
				}
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001DA994 File Offset: 0x001D8D94
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Resting".Translate() + ": x" + this.RestingMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x001DA9F4 File Offset: 0x001D8DF4
		private float RestingMultiplier(Pawn pawn)
		{
			float result;
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed))
			{
				result = this.factor;
			}
			else
			{
				result = 1f;
			}
			return result;
		}
	}
}

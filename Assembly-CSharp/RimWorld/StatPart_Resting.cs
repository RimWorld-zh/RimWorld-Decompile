using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B8 RID: 2488
	public class StatPart_Resting : StatPart
	{
		// Token: 0x060037B2 RID: 14258 RVA: 0x001DA650 File Offset: 0x001D8A50
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

		// Token: 0x060037B3 RID: 14259 RVA: 0x001DA690 File Offset: 0x001D8A90
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

		// Token: 0x060037B4 RID: 14260 RVA: 0x001DA6F0 File Offset: 0x001D8AF0
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

		// Token: 0x040023BE RID: 9150
		public float factor = 1f;
	}
}

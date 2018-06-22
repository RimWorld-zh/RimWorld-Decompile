using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class StatPart_Resting : StatPart
	{
		// Token: 0x060037AB RID: 14251 RVA: 0x001DA814 File Offset: 0x001D8C14
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

		// Token: 0x060037AC RID: 14252 RVA: 0x001DA854 File Offset: 0x001D8C54
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

		// Token: 0x060037AD RID: 14253 RVA: 0x001DA8B4 File Offset: 0x001D8CB4
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

		// Token: 0x040023B8 RID: 9144
		public float factor = 1f;
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x001DA9F8 File Offset: 0x001D8DF8
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x001DAA14 File Offset: 0x001D8E14
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (this.IsWildMan(req))
			{
				result = "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x001DAA60 File Offset: 0x001D8E60
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x040023C3 RID: 9155
		public float offset;
	}
}

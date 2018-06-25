using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BA RID: 2490
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x040023BF RID: 9151
		public float offset;

		// Token: 0x060037BF RID: 14271 RVA: 0x001DADE4 File Offset: 0x001D91E4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x001DAE00 File Offset: 0x001D9200
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

		// Token: 0x060037C1 RID: 14273 RVA: 0x001DAE4C File Offset: 0x001D924C
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}
	}
}

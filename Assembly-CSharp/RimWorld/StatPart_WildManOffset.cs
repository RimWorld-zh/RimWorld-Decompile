using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x060037C1 RID: 14273 RVA: 0x001DAACC File Offset: 0x001D8ECC
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x001DAAE8 File Offset: 0x001D8EE8
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

		// Token: 0x060037C3 RID: 14275 RVA: 0x001DAB34 File Offset: 0x001D8F34
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x040023C3 RID: 9155
		public float offset;
	}
}

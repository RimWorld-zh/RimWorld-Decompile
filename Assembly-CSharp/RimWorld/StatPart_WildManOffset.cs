using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B8 RID: 2488
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x040023BE RID: 9150
		public float offset;

		// Token: 0x060037BB RID: 14267 RVA: 0x001DACA4 File Offset: 0x001D90A4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x001DACC0 File Offset: 0x001D90C0
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

		// Token: 0x060037BD RID: 14269 RVA: 0x001DAD0C File Offset: 0x001D910C
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}
	}
}

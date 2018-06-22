using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x06003784 RID: 14212 RVA: 0x001D9C88 File Offset: 0x001D8088
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x001D9CAC File Offset: 0x001D80AC
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			string result;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				result = "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x001D9CFC File Offset: 0x001D80FC
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => (!x.RaceProps.IsFlesh) ? 0f : 1f, (ThingDef x) => (!x.race.IsFlesh) ? 0f : 1f, out bodySize);
		}
	}
}

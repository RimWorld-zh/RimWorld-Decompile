using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x06003788 RID: 14216 RVA: 0x001D9DC8 File Offset: 0x001D81C8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x001D9DEC File Offset: 0x001D81EC
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

		// Token: 0x0600378A RID: 14218 RVA: 0x001D9E3C File Offset: 0x001D823C
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => (!x.RaceProps.IsFlesh) ? 0f : 1f, (ThingDef x) => (!x.race.IsFlesh) ? 0f : 1f, out bodySize);
		}
	}
}

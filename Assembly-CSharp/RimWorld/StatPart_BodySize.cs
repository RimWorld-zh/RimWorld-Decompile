using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x0600375E RID: 14174 RVA: 0x001D9668 File Offset: 0x001D7A68
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x001D968C File Offset: 0x001D7A8C
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			string result;
			if (this.TryGetBodySize(req, out f))
			{
				result = "StatsReport_BodySize".Translate(new object[]
				{
					f.ToString("F2")
				}) + ": x" + f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x001D96E8 File Offset: 0x001D7AE8
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}

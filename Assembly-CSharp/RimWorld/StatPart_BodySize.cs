using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x0600375F RID: 14175 RVA: 0x001D8F84 File Offset: 0x001D7384
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x001D8FA8 File Offset: 0x001D73A8
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

		// Token: 0x06003761 RID: 14177 RVA: 0x001D9004 File Offset: 0x001D7404
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}

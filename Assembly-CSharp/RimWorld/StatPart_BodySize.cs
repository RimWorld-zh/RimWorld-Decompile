using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x06003761 RID: 14177 RVA: 0x001D9058 File Offset: 0x001D7458
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x001D907C File Offset: 0x001D747C
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

		// Token: 0x06003763 RID: 14179 RVA: 0x001D90D8 File Offset: 0x001D74D8
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}

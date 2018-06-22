using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A3 RID: 2467
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x0600375A RID: 14170 RVA: 0x001D9254 File Offset: 0x001D7654
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x001D9278 File Offset: 0x001D7678
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

		// Token: 0x0600375C RID: 14172 RVA: 0x001D92D4 File Offset: 0x001D76D4
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	public class StatPart_BodySize : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = default(float);
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float f = default(float);
			return (!this.TryGetBodySize(req, out f)) ? null : ("StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent());
		}

		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Func<Pawn, float>)((Pawn x) => x.BodySize), (Func<ThingDef, float>)((ThingDef x) => x.race.baseBodySize), out bodySize);
		}
	}
}

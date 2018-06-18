using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x0600379B RID: 14235 RVA: 0x001D9F1C File Offset: 0x001D831C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001D9F40 File Offset: 0x001D8340
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			string result;
			if (this.TryGetValue(req, out f))
			{
				result = "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x001D9F84 File Offset: 0x001D8384
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}

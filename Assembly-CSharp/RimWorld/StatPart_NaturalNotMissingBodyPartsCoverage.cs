using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AF RID: 2479
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x06003794 RID: 14228 RVA: 0x001DA0E0 File Offset: 0x001D84E0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x001DA104 File Offset: 0x001D8504
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

		// Token: 0x06003796 RID: 14230 RVA: 0x001DA148 File Offset: 0x001D8548
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}

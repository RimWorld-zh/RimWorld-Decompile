using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x0600376E RID: 14190 RVA: 0x001D9324 File Offset: 0x001D7724
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x001D933C File Offset: 0x001D773C
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x001D937C File Offset: 0x001D777C
		private float Multiplier(DifficultyDef d)
		{
			float result;
			switch (d.index)
			{
			case 0:
				result = this.factorRelax;
				break;
			case 1:
				result = this.factorBasebuilder;
				break;
			case 2:
				result = this.factorRough;
				break;
			case 3:
				result = this.factorChallenge;
				break;
			case 4:
				result = this.factorExtreme;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x0400239E RID: 9118
		private float factorRelax = 1f;

		// Token: 0x0400239F RID: 9119
		private float factorBasebuilder = 1f;

		// Token: 0x040023A0 RID: 9120
		private float factorRough = 1f;

		// Token: 0x040023A1 RID: 9121
		private float factorChallenge = 1f;

		// Token: 0x040023A2 RID: 9122
		private float factorExtreme = 1f;
	}
}

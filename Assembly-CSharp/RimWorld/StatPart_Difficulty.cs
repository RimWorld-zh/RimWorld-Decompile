using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x0600376C RID: 14188 RVA: 0x001D9250 File Offset: 0x001D7650
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x001D9268 File Offset: 0x001D7668
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x001D92A8 File Offset: 0x001D76A8
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

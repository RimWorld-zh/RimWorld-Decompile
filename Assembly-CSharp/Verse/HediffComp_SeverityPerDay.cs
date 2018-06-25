using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D22 RID: 3362
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x04003235 RID: 12853
		protected const int SeverityUpdateInterval = 200;

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004A14 RID: 18964 RVA: 0x00269EB4 File Offset: 0x002682B4
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x00269ED4 File Offset: 0x002682D4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(200))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.00333333341f;
				severityAdjustment += num;
			}
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x00269F18 File Offset: 0x00268318
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x00269F38 File Offset: 0x00268338
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity change per day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString();
		}
	}
}

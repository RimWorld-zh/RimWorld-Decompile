using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D24 RID: 3364
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x002689CC File Offset: 0x00266DCC
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x002689EC File Offset: 0x00266DEC
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

		// Token: 0x06004A04 RID: 18948 RVA: 0x00268A30 File Offset: 0x00266E30
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x00268A50 File Offset: 0x00266E50
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

		// Token: 0x0400322C RID: 12844
		protected const int SeverityUpdateInterval = 200;
	}
}

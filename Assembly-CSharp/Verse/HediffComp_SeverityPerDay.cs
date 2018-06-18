using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D23 RID: 3363
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x002689A4 File Offset: 0x00266DA4
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x002689C4 File Offset: 0x00266DC4
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

		// Token: 0x06004A02 RID: 18946 RVA: 0x00268A08 File Offset: 0x00266E08
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x00268A28 File Offset: 0x00266E28
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

		// Token: 0x0400322A RID: 12842
		protected const int SeverityUpdateInterval = 200;
	}
}

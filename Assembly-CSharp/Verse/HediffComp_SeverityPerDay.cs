using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000D20 RID: 3360
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x00269DD8 File Offset: 0x002681D8
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x00269DF8 File Offset: 0x002681F8
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

		// Token: 0x06004A13 RID: 18963 RVA: 0x00269E3C File Offset: 0x0026823C
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x00269E5C File Offset: 0x0026825C
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

		// Token: 0x04003235 RID: 12853
		protected const int SeverityUpdateInterval = 200;
	}
}

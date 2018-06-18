using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E08 RID: 3592
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x0600514B RID: 20811 RVA: 0x0029ABDC File Offset: 0x00298FDC
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x0029ABFC File Offset: 0x00298FFC
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x0029AC1A File Offset: 0x0029901A
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0029AC24 File Offset: 0x00299024
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}

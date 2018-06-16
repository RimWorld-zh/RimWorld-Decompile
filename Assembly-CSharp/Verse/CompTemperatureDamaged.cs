using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E09 RID: 3593
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x0600514D RID: 20813 RVA: 0x0029ABFC File Offset: 0x00298FFC
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0029AC1C File Offset: 0x0029901C
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x0029AC3A File Offset: 0x0029903A
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x0029AC44 File Offset: 0x00299044
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}

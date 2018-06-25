using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E07 RID: 3591
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x0029C2E4 File Offset: 0x0029A6E4
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x0029C304 File Offset: 0x0029A704
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0029C322 File Offset: 0x0029A722
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x0029C32C File Offset: 0x0029A72C
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}

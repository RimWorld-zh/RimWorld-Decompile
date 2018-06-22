using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E05 RID: 3589
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x0600515F RID: 20831 RVA: 0x0029C1B8 File Offset: 0x0029A5B8
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0029C1D8 File Offset: 0x0029A5D8
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0029C1F6 File Offset: 0x0029A5F6
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x0029C200 File Offset: 0x0029A600
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}

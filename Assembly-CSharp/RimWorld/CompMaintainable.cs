using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071D RID: 1821
	public class CompMaintainable : ThingComp
	{
		// Token: 0x040015F5 RID: 5621
		public int ticksSinceMaintain;

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x00157548 File Offset: 0x00155948
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x00157568 File Offset: 0x00155968
		public MaintainableStage CurStage
		{
			get
			{
				MaintainableStage result;
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					result = MaintainableStage.Healthy;
				}
				else if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					result = MaintainableStage.NeedsMaintenance;
				}
				else
				{
					result = MaintainableStage.Damaging;
				}
				return result;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x001575C4 File Offset: 0x001559C4
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.active;
			}
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x001575F4 File Offset: 0x001559F4
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x0015760C File Offset: 0x00155A0C
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksSinceMaintain++;
				if (Find.TickManager.TicksGame % 250 == 0)
				{
					this.CheckTakeDamage();
				}
			}
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x00157659 File Offset: 0x00155A59
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Active)
			{
				this.ticksSinceMaintain += 250;
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x0015768C File Offset: 0x00155A8C
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x001576D2 File Offset: 0x00155AD2
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x001576DC File Offset: 0x00155ADC
		public override string CompInspectStringExtra()
		{
			MaintainableStage curStage = this.CurStage;
			string result;
			if (curStage != MaintainableStage.NeedsMaintenance)
			{
				if (curStage != MaintainableStage.Damaging)
				{
					result = null;
				}
				else
				{
					result = "DeterioratingDueToLackOfMaintenance".Translate();
				}
			}
			else
			{
				result = "DueForMaintenance".Translate();
			}
			return result;
		}
	}
}

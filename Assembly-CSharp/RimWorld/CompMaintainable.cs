using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class CompMaintainable : ThingComp
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x0015723C File Offset: 0x0015563C
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x0015725C File Offset: 0x0015565C
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
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x001572B8 File Offset: 0x001556B8
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.active;
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x001572E8 File Offset: 0x001556E8
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x00157300 File Offset: 0x00155700
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

		// Token: 0x0600282E RID: 10286 RVA: 0x0015734D File Offset: 0x0015574D
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Active)
			{
				this.ticksSinceMaintain += 250;
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x00157380 File Offset: 0x00155780
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x001573C6 File Offset: 0x001557C6
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x001573D0 File Offset: 0x001557D0
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

		// Token: 0x040015F7 RID: 5623
		public int ticksSinceMaintain;
	}
}

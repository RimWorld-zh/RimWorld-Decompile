using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071D RID: 1821
	public class CompMaintainable : ThingComp
	{
		// Token: 0x040015F9 RID: 5625
		public int ticksSinceMaintain;

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x001577A8 File Offset: 0x00155BA8
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x001577C8 File Offset: 0x00155BC8
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
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x00157824 File Offset: 0x00155C24
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.active;
			}
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x00157854 File Offset: 0x00155C54
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x0015786C File Offset: 0x00155C6C
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

		// Token: 0x06002829 RID: 10281 RVA: 0x001578B9 File Offset: 0x00155CB9
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Active)
			{
				this.ticksSinceMaintain += 250;
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x001578EC File Offset: 0x00155CEC
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x00157932 File Offset: 0x00155D32
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x0015793C File Offset: 0x00155D3C
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

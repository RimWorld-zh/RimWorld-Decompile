using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class CompMaintainable : ThingComp
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x001571C4 File Offset: 0x001555C4
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x001571E4 File Offset: 0x001555E4
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
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x00157240 File Offset: 0x00155640
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.active;
			}
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x00157270 File Offset: 0x00155670
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x00157288 File Offset: 0x00155688
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

		// Token: 0x0600282C RID: 10284 RVA: 0x001572D5 File Offset: 0x001556D5
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Active)
			{
				this.ticksSinceMaintain += 250;
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x00157308 File Offset: 0x00155708
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x0015734E File Offset: 0x0015574E
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x00157358 File Offset: 0x00155758
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

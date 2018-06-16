using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x0015B1B4 File Offset: 0x001595B4
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x0015B1D4 File Offset: 0x001595D4
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0015B1F4 File Offset: 0x001595F4
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0015B214 File Offset: 0x00159614
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0015B230 File Offset: 0x00159630
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0015B250 File Offset: 0x00159650
		protected override bool Active
		{
			get
			{
				bool result;
				if (!base.Active)
				{
					result = false;
				}
				else
				{
					Pawn pawn = this.parent as Pawn;
					result = (pawn == null || pawn.ageTracker.CurLifeStage.shearable);
				}
				return result;
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0015B2A8 File Offset: 0x001596A8
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				result = "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
			}
			return result;
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0015B248 File Offset: 0x00159648
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0015B268 File Offset: 0x00159668
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0015B288 File Offset: 0x00159688
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0015B2A8 File Offset: 0x001596A8
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x0015B2C4 File Offset: 0x001596C4
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x0015B2E4 File Offset: 0x001596E4
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

		// Token: 0x060028B9 RID: 10425 RVA: 0x0015B33C File Offset: 0x0015973C
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

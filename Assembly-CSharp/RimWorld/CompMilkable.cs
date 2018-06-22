using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x00157B00 File Offset: 0x00155F00
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x00157B20 File Offset: 0x00155F20
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x00157B40 File Offset: 0x00155F40
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x00157B60 File Offset: 0x00155F60
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x00157B7C File Offset: 0x00155F7C
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x00157B9C File Offset: 0x00155F9C
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
					if (this.Props.milkFemaleOnly)
					{
						if (pawn != null && pawn.gender != Gender.Female)
						{
							return false;
						}
					}
					result = (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
				}
				return result;
			}
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x00157C20 File Offset: 0x00156020
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				result = "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
			}
			return result;
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000722 RID: 1826
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x00157EB0 File Offset: 0x001562B0
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x00157ED0 File Offset: 0x001562D0
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x00157EF0 File Offset: 0x001562F0
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x00157F10 File Offset: 0x00156310
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x00157F2C File Offset: 0x0015632C
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x00157F4C File Offset: 0x0015634C
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

		// Token: 0x0600283E RID: 10302 RVA: 0x00157FD0 File Offset: 0x001563D0
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

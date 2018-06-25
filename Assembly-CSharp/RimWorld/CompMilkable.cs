using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000722 RID: 1826
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x00157C50 File Offset: 0x00156050
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x00157C70 File Offset: 0x00156070
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x00157C90 File Offset: 0x00156090
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x00157CB0 File Offset: 0x001560B0
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x00157CCC File Offset: 0x001560CC
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x00157CEC File Offset: 0x001560EC
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

		// Token: 0x0600283F RID: 10303 RVA: 0x00157D70 File Offset: 0x00156170
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

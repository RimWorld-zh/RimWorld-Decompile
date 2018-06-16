using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000724 RID: 1828
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x001578CC File Offset: 0x00155CCC
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x001578EC File Offset: 0x00155CEC
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x0015790C File Offset: 0x00155D0C
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x0015792C File Offset: 0x00155D2C
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600283F RID: 10303 RVA: 0x00157948 File Offset: 0x00155D48
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x00157968 File Offset: 0x00155D68
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

		// Token: 0x06002841 RID: 10305 RVA: 0x001579EC File Offset: 0x00155DEC
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

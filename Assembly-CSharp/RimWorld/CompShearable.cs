using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000732 RID: 1842
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x0015B420 File Offset: 0x00159820
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x0015B440 File Offset: 0x00159840
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x0015B460 File Offset: 0x00159860
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x0015B480 File Offset: 0x00159880
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x0015B49C File Offset: 0x0015989C
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x0015B4BC File Offset: 0x001598BC
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

		// Token: 0x060028B2 RID: 10418 RVA: 0x0015B514 File Offset: 0x00159914
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

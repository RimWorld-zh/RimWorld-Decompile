using Verse;

namespace RimWorld
{
	public class CompShearable : CompHasGatherableBodyResource
	{
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)base.props;
			}
		}

		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = base.parent as Pawn;
				if (pawn != null && !pawn.ageTracker.CurLifeStage.shearable)
				{
					return false;
				}
				return true;
			}
		}

		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}

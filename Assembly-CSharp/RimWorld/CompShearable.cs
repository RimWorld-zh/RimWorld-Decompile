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
				bool result;
				if (!base.Active)
				{
					result = false;
				}
				else
				{
					Pawn pawn = base.parent as Pawn;
					result = ((byte)((pawn == null || pawn.ageTracker.CurLifeStage.shearable) ? 1 : 0) != 0);
				}
				return result;
			}
		}

		public override string CompInspectStringExtra()
		{
			return this.Active ? ("WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent()) : null;
		}
	}
}

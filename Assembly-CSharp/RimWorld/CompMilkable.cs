using Verse;

namespace RimWorld
{
	public class CompMilkable : CompHasGatherableBodyResource
	{
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)base.props;
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
					result = ((byte)((!this.Props.milkFemaleOnly || pawn == null || pawn.gender == Gender.Female) ? ((pawn == null || pawn.ageTracker.CurLifeStage.milkable) ? 1 : 0) : 0) != 0);
				}
				return result;
			}
		}

		public override string CompInspectStringExtra()
		{
			return this.Active ? ("MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent()) : null;
		}
	}
}

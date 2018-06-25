using System;
using Verse;

namespace RimWorld
{
	public class CompMilkable : CompHasGatherableBodyResource
	{
		public CompMilkable()
		{
		}

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
				return (CompProperties_Milkable)this.props;
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

using Verse;

namespace RimWorld
{
	public class CompMaintainable : ThingComp
	{
		public int ticksSinceMaintain = 0;

		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)base.props;
			}
		}

		public MaintainableStage CurStage
		{
			get
			{
				return (MaintainableStage)((this.ticksSinceMaintain >= this.Props.ticksHealthy) ? ((this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance) ? 1 : 2) : 0);
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		public override void CompTickRare()
		{
			Hive hive = base.parent as Hive;
			if (hive != null && !hive.active)
				return;
			this.ticksSinceMaintain += 250;
			if (this.CurStage == MaintainableStage.Damaging)
			{
				base.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
		}

		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		public override string CompInspectStringExtra()
		{
			string result;
			switch (this.CurStage)
			{
			case MaintainableStage.NeedsMaintenance:
			{
				result = "DueForMaintenance".Translate();
				break;
			}
			case MaintainableStage.Damaging:
			{
				result = "DeterioratingDueToLackOfMaintenance".Translate();
				break;
			}
			default:
			{
				result = (string)null;
				break;
			}
			}
			return result;
		}
	}
}

using Verse;

namespace RimWorld
{
	public class CompMaintainable : ThingComp
	{
		public int ticksSinceMaintain;

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
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					return MaintainableStage.Healthy;
				}
				if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					return MaintainableStage.NeedsMaintenance;
				}
				return MaintainableStage.Damaging;
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
			switch (this.CurStage)
			{
			case MaintainableStage.NeedsMaintenance:
				return "DueForMaintenance".Translate();
			case MaintainableStage.Damaging:
				return "DeterioratingDueToLackOfMaintenance".Translate();
			default:
				return null;
			}
		}
	}
}

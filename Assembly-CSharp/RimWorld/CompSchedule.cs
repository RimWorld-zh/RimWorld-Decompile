using Verse;

namespace RimWorld
{
	public class CompSchedule : ThingComp
	{
		public const string ScheduledOnSignal = "ScheduledOn";

		public const string ScheduledOffSignal = "ScheduledOff";

		private bool intAllowed = false;

		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)base.props;
			}
		}

		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed != value)
				{
					this.intAllowed = value;
					base.parent.BroadcastCompSignal((!this.intAllowed) ? "ScheduledOff" : "ScheduledOn");
				}
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(base.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
			}
			else
			{
				this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
			}
		}

		public override string CompInspectStringExtra()
		{
			return this.Allowed ? null : this.Props.offMessage;
		}
	}
}

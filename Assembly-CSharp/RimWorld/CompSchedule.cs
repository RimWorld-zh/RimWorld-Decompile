using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000423 RID: 1059
	public class CompSchedule : ThingComp
	{
		// Token: 0x04000B35 RID: 2869
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04000B36 RID: 2870
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04000B37 RID: 2871
		private bool intAllowed = false;

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x0009FC34 File Offset: 0x0009E034
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0009FC54 File Offset: 0x0009E054
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x0009FC6F File Offset: 0x0009E06F
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
					this.parent.BroadcastCompSignal((!this.intAllowed) ? "ScheduledOff" : "ScheduledOn");
				}
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0009FCAF File Offset: 0x0009E0AF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0009FCBF File Offset: 0x0009E0BF
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0009FCD0 File Offset: 0x0009E0D0
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
			}
			else
			{
				this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
			}
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0009FD60 File Offset: 0x0009E160
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Allowed)
			{
				result = this.Props.offMessage;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}

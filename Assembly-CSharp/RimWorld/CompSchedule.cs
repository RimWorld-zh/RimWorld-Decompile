using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000421 RID: 1057
	public class CompSchedule : ThingComp
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0009F700 File Offset: 0x0009DB00
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x0009F720 File Offset: 0x0009DB20
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x0009F73B File Offset: 0x0009DB3B
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

		// Token: 0x0600126B RID: 4715 RVA: 0x0009F77B File Offset: 0x0009DB7B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0009F78B File Offset: 0x0009DB8B
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0009F79C File Offset: 0x0009DB9C
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

		// Token: 0x0600126E RID: 4718 RVA: 0x0009F82C File Offset: 0x0009DC2C
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

		// Token: 0x04000B31 RID: 2865
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04000B32 RID: 2866
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04000B33 RID: 2867
		private bool intAllowed = false;
	}
}
